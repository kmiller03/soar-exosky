from astroquery.gaia import Gaia
import pandas as pd
from astropy.coordinates import Distance, SkyCoord
from astropy import units as u
import requests
import io
from models.exo import Exo
from models.star import Star
from tqdm import tqdm

def star_setup():
    # Check if the file 'gaia_star_data.csv' already exists
    try:
        df = pd.read_csv('gaia_star_data.csv')
        print("Data successfully read from gaia_star_data.csv")
    except FileNotFoundError:
        print("No existing CSV found. Running query...")

        # Query to retrieve at least 2000 stars with bp_rp < 0
        query_bp_rp_negative = """
        SELECT TOP 2000
            source_id,
            ra,
            dec,
            parallax,
            phot_rp_mean_mag,  
            phot_g_mean_mag,
            phot_bp_mean_mag
        FROM gaiadr3.gaia_source
        WHERE (MOD(source_id, 10000) = 0) AND parallax > 0 AND bp_rp < 0
        """

        # Query to retrieve remaining 8000 stars with any bp_rp value
        query_remaining_stars = """
        SELECT TOP 8000
            source_id,
            ra,
            dec,
            parallax,
            phot_rp_mean_mag,  
            phot_g_mean_mag,
            phot_bp_mean_mag
        FROM gaiadr3.gaia_source
        WHERE (MOD(source_id, 1000000) = 0) AND parallax > 0
        """

        # Execute both queries asynchronously
        job_bp_rp_negative = Gaia.launch_job_async(query_bp_rp_negative)
        results_bp_rp_negative = job_bp_rp_negative.get_results()

        print("First query completed. Starting second query...")
        
        job_remaining_stars = Gaia.launch_job_async(query_remaining_stars)
        results_remaining_stars = job_remaining_stars.get_results()

        # Convert both results to pandas DataFrames
        df_bp_rp_negative = results_bp_rp_negative.to_pandas()
        df_remaining_stars = results_remaining_stars.to_pandas()

        # Combine the two DataFrames
        df = pd.concat([df_bp_rp_negative, df_remaining_stars], ignore_index=True)

        # Save the combined DataFrame to a CSV file
        df.to_csv('gaia_star_data.csv', index=False)
        print("Data successfully saved to gaia_star_data.csv")

    # Create Star objects from the DataFrame
    stars = []
    for index, row in df.iterrows():
        parallax_value = df.iloc[index, 3]
        if pd.isna(parallax_value) or parallax_value == 0.0 or parallax_value < 0:
            continue

        # Create the Star object and calculate distance from parallax
        new_star = Star(
            df.iloc[index, 0],  # source_id
            df.iloc[index, 1],  # ra
            df.iloc[index, 2],  # dec
            Distance(parallax=parallax_value * u.mas),  # parallax to distance
            df.iloc[index,4],
            df.iloc[index,5],
            df.iloc[index,6]
        )
        stars.append(new_star)

    print(f'Finished Querying {len(stars)} Stars')
    return stars



def exo_setup():
    try:
        # Load the exoplanet data
        df_exo = pd.read_csv('all_planets_data.csv')
    except FileNotFoundError:
        # Exoplanet Archive API request to retrieve all exoplanets
        base_url = 'https://exoplanetarchive.ipac.caltech.edu/TAP/sync?query='
        query = 'select+pl_name,ra,dec,sy_dist+from+ps'
        full_url = base_url + query + '&format=csv'

        response = requests.get(full_url)

        # Check if the request was successful
        if response.status_code == 200:
            data = response.text
            df_exo = pd.read_csv(io.StringIO(data))
            df_exo.to_csv('all_planets_data.csv', index=False)
            print("Data successfully saved to all_planets_data.csv")
        else:
            print("Error:", response.status_code)

    exos = []

    # Create Exo objects from the CSV file
    for index, row in df_exo.iterrows():
        if index == 0:
            continue

        new_exo = Exo(
            df_exo.iloc[index, 0],  # planet name
            df_exo.iloc[index, 1],  # ra
            df_exo.iloc[index, 2],  # dec
            df_exo.iloc[index, 3]   # distance
        )
        if index == 5:
            print(f'New exo: {new_exo.get_id()}, {new_exo.get_ra()}, {new_exo.get_dec()}, {new_exo.get_dist()}')
        exos.append(new_exo)

    return exos




# Function to calculate the new RA and Dec of stars relative to an exoplanet

def return_new_stars(exo, stars):
    exoplanet_coord = SkyCoord(ra=exo.get_ra() * u.degree, dec=exo.get_dec() * u.degree, 
                               distance=exo.get_dist() * u.parsec, frame='icrs')
    new_stars = []
    max_rgb = [0,0,0]

    for star in stars:
        star_coord = SkyCoord(ra=star.get_ra() * u.degree, dec=star.get_dec() * u.degree, 
                              distance=star.get_dist().to(u.parsec), frame='icrs')

        # Calculate the star's position relative to the exoplanet
        star_relative_to_exoplanet = star_coord.transform_to(exoplanet_coord.skyoffset_frame())
        
        # Calculate the 3D distance between the star and the exoplanet
        separation = star_coord.separation_3d(exoplanet_coord)

        new_star = Star(star.get_id(), star_relative_to_exoplanet.lon.deg, 
                        star_relative_to_exoplanet.lat.deg, separation.to(u.parsec), star.get_rmag(), star.get_gmag(), star.get_bmag())
        if star.get_rmag() > max_rgb[0]:
            max_rgb[0] = star.get_rmag()
        if star.get_gmag() > max_rgb[1]:
            max_rgb[1] = star.get_gmag()
        if star.get_bmag() > max_rgb[2]:
            max_rgb[2] = star.get_bmag()

        # Create a new Star object with the relative RA and Dec and 3D separation distance
        # new_star = Star(star.get_id(), star_relative_to_exoplanet.lon.deg, 
        #                star_relative_to_exoplanet.lat.deg, separation.to(u.parsec))

        new_stars.append(new_star)

    return new_stars, max_rgb


def return_exo_by_id(exo_id):
    for exo in exo_setup():
        if exo.get_id() == exo_id:
            return exo
    print("Exo id not found")
    return None

#def convert_rgbmag():
     




