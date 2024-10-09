# Use the official Python 3.12 image
FROM python:3.12-slim

# Set the working directory inside the container
WORKDIR /app

# Copy the current directory contents into the container at /app
COPY . /app

# Install any needed packages specified in requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# Expose the port that Flask will run on
EXPOSE 5000

# Define environment variables
ENV FLASK_ENV=production
ENV APP_PORT=5000

# Run the application using Gunicorn and Flask-SocketIO
# CMD ["gunicorn", "--worker-class", "gevent", "-w", "1", "-b", "0.0.0.0:5000", "wsgi:app"]
CMD ["gunicorn", "--worker-class", "gevent", "-w", "1", "-b", "0.0.0.0:5000", "--timeout", "600", "wsgi:app"]

