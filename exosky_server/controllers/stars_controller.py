from flask import Blueprint, render_template, send_from_directory, jsonify
from flask_socketio import emit
from services import get_star_data as gsd
from services import get_exoplanets as gep
import os

bp = Blueprint("stars_controller", __name__)
socketio = None


@bp.route("/exoplanets")
def get_exoplanets():
    '''
        {
            exoplanetSets: [["planetA", "planetB", "planetC"], ["planetD", "planetE", "planetF"]]
        }
    '''
    sets = []
    for i in range(10):
        sets.append(gep.return_planet_names(i))
    
    return jsonify({'exoplanetSets': sets}), 200
        
    


@bp.route("/stars_rel_to/<string:planet_id>")
def get_stars_rel_to(planet_id):
    print(f'Request received for exoplanet {planet_id}')
    try:
        # exos = gsd.exo_setup()
        first_exo = gsd.return_exo_by_id(planet_id)
        if first_exo is None:
            return jsonify({'error': 'Exoplanet not found'}), 404
        stars_rel_to, max_rgb = gsd.return_new_stars(first_exo, gsd.star_setup())
        # stars_rel_to = gsd.get_stars(exo, stars)
        '''
        return to the client:
        {
            'expoplanet_id': planet_id,
            'stars': stars_rel_to
        }
        leverage this function for the list of stars objects:
            def to_json(self):
                return {
                    "id": self._id,
                    "ra": self._ra,
                    "dec": self._dec,
                    "dist": self._dist
                }
        '''
        res = {
            'expoplanet_id': planet_id,
            'stars': [star.to_json() for star in stars_rel_to],
            'max_rgb': [float(max_rgb[0]), float(max_rgb[1]), float(max_rgb[2])]

        }
        print("Returning stars")
        # convert response to json and send to client
        return jsonify(res), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500