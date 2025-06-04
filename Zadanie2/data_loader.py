# data_loader.py
import os
import pandas as pd
from glob import glob
from typing import List, Tuple


class DataLoader:
    """Handles loading of measurement data from specified directories."""

    def __init__(self, base_dir: str):
        """
        Initialize data loader with base directory.

        Args:
            base_dir: Path to directory containing F8 and F10 folders
        """
        self.base_path = os.path.abspath(base_dir)
        self.room_paths = {
            'F8': os.path.join(self.base_path, 'F8'),
            'F10': os.path.join(self.base_path, 'F10')
        }

        self._validate_directories()

    def _validate_directories(self) -> None:
        """Check if required directories exist."""
        print("\nData directories:")
        for room, path in self.room_paths.items():
            exists = os.path.exists(path)
            print(f"{room} directory: {path} {'(found)' if exists else '(missing)'}")
            if not exists:
                raise FileNotFoundError(f"Required directory not found: {path}")

    def _load_room_files(self, room: str, pattern: str) -> List[pd.DataFrame]:
        """
        Load Excel files matching pattern from specified room.

        Args:
            room: Room identifier ('F8' or 'F10')
            pattern: File pattern to match (e.g., '*stat*')

        Returns:
            List of DataFrames containing measurement data
        """
        room_dir = self.room_paths[room]
        search_pattern = os.path.join(room_dir, f"{room.lower()}_{pattern}.xlsx")
        file_list = glob(search_pattern)

        print(f"\nLoading {pattern} files from {room}:")
        print(f"Found {len(file_list)} files matching {search_pattern}")

        data = []
        for file in file_list:
            try:
                df = pd.read_excel(file)
                df['source_file'] = os.path.basename(file)
                df['room'] = room
                data.append(df)
            except Exception as e:
                print(f"Error loading {file}: {e}")

        return data

    def load_measurements(self, measurement_type: str) -> Tuple[List[pd.DataFrame], List[pd.DataFrame]]:
        """
        Load measurement data of specified type from both rooms.

        Args:
            measurement_type: Type of measurement ('stat', 'random' or 'dynamic')

        Returns:
            Tuple of (F8_data, F10_data) lists of DataFrames
        """
        if measurement_type == 'dynamic':
            f8_data = self._load_room_files('F8', '*[!stat][!random]*')
            f10_data = self._load_room_files('F10', '*[!stat][!random]*')
        else:
            f8_data = self._load_room_files('F8', f'{measurement_type}*')
            f10_data = self._load_room_files('F10', f'{measurement_type}*')

        return f8_data, f10_data

    def get_data_summary(self) -> pd.DataFrame:
        """Generate summary of available data."""
        summary = []

        for room in ['F8', 'F10']:
            for m_type in ['stat', 'random', 'dynamic']:
                data = self._load_room_files(room,
                                             '*[!stat][!random]*' if m_type == 'dynamic'
                                             else f'{m_type}*')
                summary.append({
                    'room': room,
                    'type': m_type,
                    'file_count': len(data),
                    'total_rows': sum(len(df) for df in data)
                })

        return pd.DataFrame(summary)