import pandas as pd
import matplotlib.pyplot as plt

# Load CSV data
file_path = 'gaia_star_data.csv'  # Replace with the actual path to your CSV file
data = pd.read_csv(file_path)
data['bp_rp_calc'] = data['phot_bp_mean_mag'] - data['phot_rp_mean_mag']


# Calculate temperature using the bp_rp index
data['temperature'] = 5600 * (1 / (0.92 * (data['bp_rp_calc'] + 1)))

# Classify stars based on temperature into spectral types and assign colors
def classify_star(temperature):
    if temperature >= 30000:
        return 'O', 'blue'
    elif 10000 <= temperature < 30000:
        return 'B', 'blueviolet'
    elif 7500 <= temperature < 10000:
        return 'A', 'lightsteelblue'
    elif 6000 <= temperature < 7500:
        return 'F', 'lightyellow'
    elif 5200 <= temperature < 6000:
        return 'G', 'yellow'
    elif 3700 <= temperature < 5200:
        return 'K', 'orange'
    else:
        return 'M', 'red'

# Apply classification to each star and create columns for spectral type and color
data[['spectral_type', 'color']] = data['temperature'].apply(lambda temp: pd.Series(classify_star(temp)))

# Create a new dataframe for plotting
plot_df = data[['bp_rp_calc', 'temperature', 'spectral_type']]

# Define row index for x-axis
x = range(len(plot_df))

# Create a plot with 3 subplots
fig, axes = plt.subplots(3, 1, figsize=(12, 18))

# Graph 1: Temperature vs. Row Index
axes[0].plot(x, plot_df['temperature'], label='Temperature (K)', color='orange')
axes[0].set_xlabel('Row Index')
axes[0].set_ylabel('Temperature (K)')
axes[0].set_title('Star Temperature vs. Row Index')
axes[0].grid()
axes[0].legend()

# Graph 2: bp_rp vs. Row Index
axes[1].plot(x, plot_df['bp_rp_calc'], label='bp_rp', color='blue')
axes[1].set_xlabel('Row Index')
axes[1].set_ylabel('bp_rp')
axes[1].set_title('Star bp_rp vs. Row Index')
axes[1].grid()
axes[1].legend()

# Graph 3: Spectral Classification vs. Row Index
# We map spectral types to Y-axis values (O=0, B=1, ..., M=6)
spectral_mapping = {'O': 0, 'B': 1, 'A': 2, 'F': 3, 'G': 4, 'K': 5, 'M': 6}
plot_df['spectral_type_index'] = plot_df['spectral_type'].map(spectral_mapping)

# Plot with colors corresponding to spectral type
axes[2].scatter(x, plot_df['spectral_type_index'], c=data['color'], s=50, label='Spectral Type')
axes[2].set_xlabel('Row Index')
axes[2].set_ylabel('Spectral Type')
axes[2].set_title('Star Spectral Type vs. Row Index')
axes[2].set_yticks(list(spectral_mapping.values()))  # Set Y-axis ticks to spectral indices
axes[2].set_yticklabels(list(spectral_mapping.keys()))  # Map indices to labels (O, B, A, etc.)
axes[2].grid()
axes[2].legend()

# Adjust layout and show the plot
plt.tight_layout()
plt.show()
