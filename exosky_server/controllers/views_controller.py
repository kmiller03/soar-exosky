from flask import Blueprint, render_template, send_from_directory
from flask_socketio import emit
import os

bp = Blueprint("views_controller", __name__)
socketio = None

@bp.route("/ping")
def ping():
    return "pong"
@bp.route("/")
def default():
    return render_template("index.html")
@bp.route("/gui/<path:subpath>")
def serve_tab(subpath):
    return render_template("index.html")

@bp.route('/TemplateData/<path:filename>')
def serve_templatedata_folder(filename):
    return send_from_directory(os.path.join('templates', 'TemplateData'), filename)

@bp.route('/Build/<path:filename>')
def serve_build_folder(filename):
    return send_from_directory(os.path.join('templates', 'Build'), filename)

@bp.route('/manifest.json')
def serve_manifest():
    return send_from_directory('templates', 'manifest.json')

@bp.route('/favicon.png')
def serve_favicon():
    return send_from_directory('templates', 'favicon.png')

def register_socketio_events(_socketio):
    global socketio
    socketio = _socketio
    @_socketio.on('ping')
    def handle_ping():
        print("Ping received")
        emit('pong', {'message': 'pong'})
