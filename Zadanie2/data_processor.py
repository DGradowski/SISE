# data_processor.py
import numpy as np
from sklearn.preprocessing import StandardScaler
from sklearn.model_selection import train_test_split
from typing import Tuple


class DataPreprocessor:
    """Handles data preprocessing and sequence generation for time series."""

    def __init__(self, window_size: int = 5):
        """
        Initialize preprocessor.

        Args:
            window_size: Number of time steps to use for prediction
        """
        self.window_size = window_size
        self.scaler = StandardScaler()

    def create_sequences(self, data: np.ndarray) -> Tuple[np.ndarray, np.ndarray]:
        """
        Create input-output sequences from time series data.

        Args:
            data: 1D array of measurement values

        Returns:
            Tuple of (input_sequences, target_values)
        """
        inputs, targets = [], []

        for i in range(len(data) - self.window_size):
            inputs.append(data[i:i + self.window_size])
            targets.append(data[i + self.window_size])

        return np.array(inputs), np.array(targets)

    def prepare_datasets(self,
                         static_data: np.ndarray,
                         dynamic_data: np.ndarray,
                         test_ratio: float = 0.2) -> dict:
        """
        Prepare training, validation and test datasets.

        Args:
            static_data: Data for training/validation
            dynamic_data: Data for testing
            test_ratio: Fraction of static data to use for validation

        Returns:
            Dictionary containing prepared datasets
        """
        # Scale all data
        static_scaled = self.scaler.fit_transform(static_data.reshape(-1, 1)).flatten()
        dynamic_scaled = self.scaler.transform(dynamic_data.reshape(-1, 1)).flatten()

        # Create sequences
        x_stat, y_stat = self.create_sequences(static_scaled)
        x_dyn, y_dyn = self.create_sequences(dynamic_scaled)

        # Split static data into train/validation
        x_train, x_val, y_train, y_val = train_test_split(
            x_stat, y_stat, test_size=test_ratio, random_state=42
        )

        return {
            'train': (x_train, y_train),
            'val': (x_val, y_val),
            'test': (x_dyn, y_dyn),
            'scaler': self.scaler
        }

    def inverse_scale(self, data: np.ndarray) -> np.ndarray:
        """Transform scaled data back to original scale."""
        return self.scaler.inverse_transform(data.reshape(-1, 1)).flatten()