# main.py
import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import torch
from torch.utils.data import DataLoader, TensorDataset

from data_loader import DataLoader as MeasurementDataLoader
from data_processor import DataPreprocessor
from model import LSTMCorrectionModel
from trainer import ModelTrainer


def setup_device() -> torch.device:
    """Initialize and return available computing device."""
    device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
    print(f"\nUsing device: {device}")
    return device


def prepare_tensor_datasets(data_dict: dict) -> dict:
    """Convert numpy arrays to PyTorch datasets."""
    return {
        'train': TensorDataset(
            torch.FloatTensor(data_dict['train'][0]),
            torch.FloatTensor(data_dict['train'][1])
        ),
        'val': TensorDataset(
            torch.FloatTensor(data_dict['val'][0]),
            torch.FloatTensor(data_dict['val'][1])
        ),
        'test': TensorDataset(
            torch.FloatTensor(data_dict['test'][0]),
            torch.FloatTensor(data_dict['test'][1])
        )
    }


def create_data_loaders(datasets: dict, batch_size: int = 32) -> dict:
    """Create data loaders from datasets."""
    return {
        'train': DataLoader(
            datasets['train'], batch_size=batch_size, shuffle=True),
        'val': DataLoader(
            datasets['val'], batch_size=batch_size),
        'test': DataLoader(
            datasets['test'], batch_size=batch_size)
    }


def analyze_results(original: np.ndarray,
                    corrected: np.ndarray,
                    model: torch.nn.Module,
                    save_path: str = 'results') -> None:
    """Generate and save analysis of correction results."""
    os.makedirs(save_path, exist_ok=True)

    # Calculate errors
    original_mean = original.mean()
    corrected_mean = corrected.mean()
    original_errors = np.abs(original - original_mean)
    corrected_errors = np.abs(corrected - corrected_mean)

    # Get model weights statistics
    weight_stats = model.analyze_weights()

    # Error statistics
    stats = {
        'original_mean': original_mean,
        'corrected_mean': corrected_mean,
        'original_error_mean': original_errors.mean(),
        'original_error_std': original_errors.std(),
        'corrected_error_mean': corrected_errors.mean(),
        'corrected_error_std': corrected_errors.std(),
        'improvement': (original_errors.std() - corrected_errors.std()) / original_errors.std() * 100,
        'lstm_weight_mean': weight_stats['lstm.weight_ih_l0']['mean'],
        'lstm_weight_std': weight_stats['lstm.weight_ih_l0']['std'],
        'output_weight_mean': weight_stats['linear.weight']['mean'],
        'output_weight_std': weight_stats['linear.weight']['std']
    }

    # Save statistics
    pd.DataFrame([stats]).to_csv(os.path.join(save_path, 'error_stats.csv'), index=False)

    # Plot error distributions
    plt.figure(figsize=(10, 6))
    plt.hist(original_errors, bins=50, alpha=0.5, label='Original Errors')
    plt.hist(corrected_errors, bins=50, alpha=0.5, label='Corrected Errors')
    plt.xlabel('Error Magnitude')
    plt.ylabel('Frequency')
    plt.title('Error Distribution Comparison')
    plt.legend()
    plt.savefig(os.path.join(save_path, 'error_distribution.png'))
    plt.close()

    # Plot CDF
    sorted_errors = np.sort(corrected_errors)
    cdf = np.arange(1, len(sorted_errors) + 1) / len(sorted_errors)
    cdf_df = pd.DataFrame({'error': sorted_errors, 'cdf': cdf})
    cdf_df.to_csv(os.path.join(save_path, 'error_cdf.csv'), index=False)

    # Save CDF plot
    plt.figure(figsize=(10, 6))
    plt.plot(sorted_errors, cdf)
    plt.xlabel('Error Magnitude')
    plt.ylabel('CDF')
    plt.title('Cumulative Distribution Function of Corrected Errors')
    plt.savefig(os.path.join(save_path, 'error_cdf.png'))
    plt.close()

    # Save CDF values to Excel
    pd.DataFrame({'cdf': cdf}).to_excel(
        os.path.join(save_path, 'error_cdf.xlsx'),
        index=False,
        header=['Dystrybuanta błędu']
    )


def main():
    # Setup
    device = setup_device()
    torch.manual_seed(42)
    np.random.seed(42)

    # Load data
    print("\nLoading measurement data...")
    data_loader = MeasurementDataLoader('pomiary')
    summary = data_loader.get_data_summary()
    print("\nData summary:")
    print(summary)

    # Prepare datasets
    print("\nPreprocessing data...")
    f8_static, f10_static = data_loader.load_measurements('stat')
    f8_dynamic, f10_dynamic = data_loader.load_measurements('dynamic')

    static_data = np.concatenate([df.iloc[:, 0].values for df in f8_static + f10_static])
    dynamic_data = np.concatenate([df.iloc[:, 0].values for df in f8_dynamic + f10_dynamic])

    preprocessor = DataPreprocessor(window_size=5)
    data_dict = preprocessor.prepare_datasets(static_data, dynamic_data)
    datasets = prepare_tensor_datasets(data_dict)
    loaders = create_data_loaders(datasets)

    # Initialize model
    print("\nInitializing model...")
    model = LSTMCorrectionModel(
        input_dim=1,
        hidden_dim=50,
        num_layers=2,
        output_dim=1
    )

    trainer = ModelTrainer(model, device)
    criterion = torch.nn.MSELoss()
    optimizer = torch.optim.Adam(model.parameters(), lr=0.0005)

    # Train model
    print("\nTraining model...")
    train_losses, val_losses = trainer.train(
        loaders['train'],
        loaders['val'],
        criterion,
        optimizer,
        epochs=200,
        patience=15
    )

    # Plot training progress
    plt.figure(figsize=(10, 6))
    plt.plot(train_losses, label='Training Loss')
    plt.plot(val_losses, label='Validation Loss')
    plt.xlabel('Epoch')
    plt.ylabel('Loss')
    plt.title('Training Progress')
    plt.legend()
    plt.savefig('training_progress.png')
    plt.close()

    # Evaluate on test set
    print("\nEvaluating model...")
    test_loss, predictions = trainer.evaluate(loaders['test'], criterion)
    print(f"Test Loss: {test_loss:.4f}")

    # Get additional statistics
    original_values = data_dict['test'][1].flatten()
    corrected_values = preprocessor.inverse_scale(predictions.flatten())
    original_unscaled = preprocessor.inverse_scale(original_values)

    # Analyze results with model
    analyze_results(original_unscaled, corrected_values, model)

    # Print final statistics
    print("\nFinal Statistics:")
    stats = pd.read_csv('results/error_stats.csv').iloc[0]
    print(f"Training Loss: {train_losses[-1]:.4f}")
    print(f"Validation Loss: {val_losses[-1]:.4f}")
    print(f"Test Loss: {test_loss:.4f}")
    print(f"Original Error Mean: {stats['original_error_mean']:.4f}")
    print(f"Corrected Error Mean: {stats['corrected_error_mean']:.4f}")
    print(f"Improvement: {stats['improvement']:.2f}%")
    print(f"LSTM Weights - Mean: {stats['lstm_weight_mean']:.4f}, Std: {stats['lstm_weight_std']:.4f}")
    print(f"Output Weights - Mean: {stats['output_weight_mean']:.4f}, Std: {stats['output_weight_std']:.4f}")

    # Save model
    torch.save(model.state_dict(), 'measurement_correction_model.pth')
    print("\nTraining complete. Model saved.")


if __name__ == '__main__':
    main()