from models.star import *
import random

def get_stars(exo, stars):
    # generate a list of random stars
    # constructor is def __init__(self, idnum, ra, dec, dist):
    # idnum should be an increasing integer, ra should be between 0 and 360, dec should be between -90 and 90, dist should be between 0 and 1000
    star_list = []
    for i in range(1000):
        ra = random.uniform(0, 360)
        dec = random.uniform(-90, 90)
        dist = random.uniform(0, 1000)
        star_list.append(Star(i, ra, dec, dist))

    return star_list
        
