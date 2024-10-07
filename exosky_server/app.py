# app.py
from flask import Flask, render_template
from flask_cors import CORS
from flask_socketio import SocketIO
from controllers import views_controller
from controllers import stars_controller

# Create Flask app instance
app = Flask(__name__)
CORS(app)

# Configure SocketIO
socketio = SocketIO(app, cors_allowed_origins="*")

app.register_blueprint(views_controller.bp)
app.register_blueprint(stars_controller.bp)

views_controller.register_socketio_events(socketio)

# Run the server using socketio's wrapper
if __name__ == '__main__':
    socketio.run(app)
