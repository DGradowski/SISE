import torch
import torch.nn as nn
import numpy as np
from typing import Tuple


class LSTMCorrectionModel(nn.Module):
    """LSTM-based model for measurement correction."""

    def __init__(self,
                 input_dim: int = 1,
                 hidden_dim: int = 50,
                 num_layers: int = 2,
                 output_dim: int = 1):
        super().__init__()

        self.hidden_dim = hidden_dim
        self.num_layers = num_layers

        # LSTM layer
        self.lstm = nn.LSTM(
            input_size=input_dim,
            hidden_size=hidden_dim,
            num_layers=num_layers,
            batch_first=True
        )

        # Output layer
        self.linear = nn.Linear(hidden_dim, output_dim)

        # Initialize weights
        self._init_weights()

    def _init_weights(self):
        """Initialize weights with Xavier/Glorot initialization"""
        for name, param in self.named_parameters():
            if 'weight' in name:
                nn.init.xavier_normal_(param)
            elif 'bias' in name:
                nn.init.constant_(param, 0.0)

    def forward(self, x: torch.Tensor) -> torch.Tensor:
        if x.ndim == 2:
            x = x.unsqueeze(-1)

        h0 = torch.zeros(self.num_layers, x.size(0), self.hidden_dim).to(x.device)
        c0 = torch.zeros(self.num_layers, x.size(0), self.hidden_dim).to(x.device)

        lstm_out, _ = self.lstm(x, (h0, c0))
        predictions = self.linear(lstm_out[:, -1, :])

        return predictions

    def analyze_weights(self):
        """Analyze and return weight statistics"""
        stats = {}
        for name, param in self.named_parameters():
            if param.requires_grad:
                data = param.data.cpu().numpy()
                stats[name] = {
                    'mean': np.mean(data),
                    'std': np.std(data)
                }
        return stats