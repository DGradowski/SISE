# trainer.py
import torch
from torch.utils.data import DataLoader
import numpy as np
from typing import Tuple, List


class ModelTrainer:
    """Handles model training and evaluation."""

    def __init__(self, model: torch.nn.Module, device: torch.device):
        """
        Initialize trainer.

        Args:
            model: Model to train
            device: Device to use for training (CPU/GPU)
        """
        self.model = model.to(device)
        self.device = device

    def train_epoch(self,
                    loader: DataLoader,
                    criterion: torch.nn.Module,
                    optimizer: torch.optim.Optimizer) -> float:
        """
        Train model for one epoch.

        Returns:
            Average loss for the epoch
        """
        self.model.train()
        total_loss = 0.0

        for inputs, targets in loader:
            inputs, targets = inputs.to(self.device), targets.to(self.device)

            optimizer.zero_grad()
            outputs = self.model(inputs)
            loss = criterion(outputs, targets)
            loss.backward()
            optimizer.step()

            total_loss += loss.item()

        return total_loss / len(loader)

    def evaluate(self,
                 loader: DataLoader,
                 criterion: torch.nn.Module) -> Tuple[float, np.ndarray]:
        """
        Evaluate model on given data.

        Returns:
            Tuple of (average loss, model predictions)
        """
        self.model.eval()
        total_loss = 0.0
        all_preds = []

        with torch.no_grad():
            for inputs, targets in loader:
                inputs, targets = inputs.to(self.device), targets.to(self.device)
                outputs = self.model(inputs)

                loss = criterion(outputs, targets)
                total_loss += loss.item()
                all_preds.append(outputs.cpu().numpy())

        avg_loss = total_loss / len(loader)
        predictions = np.concatenate(all_preds)

        return avg_loss, predictions

    def train(self,
              train_loader: DataLoader,
              val_loader: DataLoader,
              criterion: torch.nn.Module,
              optimizer: torch.optim.Optimizer,
              epochs: int,
              patience: int = 10) -> Tuple[List[float], List[float]]:
        """
        Full training loop with early stopping.

        Returns:
            Tuple of (train_losses, val_losses)
        """
        best_loss = float('inf')
        no_improve = 0
        train_losses = []
        val_losses = []

        for epoch in range(epochs):
            train_loss = self.train_epoch(train_loader, criterion, optimizer)
            val_loss, _ = self.evaluate(val_loader, criterion)

            train_losses.append(train_loss)
            val_losses.append(val_loss)

            # Early stopping check
            if val_loss < best_loss:
                best_loss = val_loss
                no_improve = 0
            else:
                no_improve += 1

            if no_improve >= patience:
                print(f"Early stopping at epoch {epoch + 1}")
                break

            if (epoch + 1) % 10 == 0:
                print(f"Epoch {epoch + 1}/{epochs} - "
                      f"Train Loss: {train_loss:.4f}, Val Loss: {val_loss:.4f}")

        return train_losses, val_losses