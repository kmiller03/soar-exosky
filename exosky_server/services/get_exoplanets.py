import pandas as pd

def return_planet_names(planetPageIndex):

    #load planet data
    df = pd.read_csv('all_planets_data.csv')

    planetNameList = []

    for index, row in df.iterrows():
        if index > (planetPageIndex + 1) * 10:
            break
        if index >= ( (planetPageIndex * 10) + 1): 
            planetName = df.iloc[index, 0]
            planetNameList.append(planetName)

    return planetNameList