# install dependencies
#    pip install -r requirements_env_flask.txt
# self host with
#    python flask.py
# make webhook using
#    ngrok http 5000
# add webhook to app and trigger it
import hashlib
import hmac
import json

from flask import Flask
from flask import request, url_for
app = Flask(__name__)

@app.route('/', methods=['POST'])
def webhook_flash():
    KEY = "secret"

    DATA = request.get_data()
    print "===============================================================";
    print DATA;
    print "===============================================================";
    EXPECTED = ""
    if not request.headers.get('X-Hub-Signature') is None :
        EXPECTED = str(request.headers.get('X-Hub-Signature'))
    if not EXPECTED :
        print("Not signed. Not calculating");
    else :
        calculated = hmac.new(KEY, DATA, hashlib.sha1).hexdigest()
        calculated = "sha1=" + (calculated)
        print("Expected  : " + EXPECTED);
        print("Calculated: " + calculated);
        print("Match?    : " + str(calculated == EXPECTED));
    pass
    output = json.loads(DATA)
    print("Topic Recieved: " + output["topic"]);

if __name__ == '__main__':
    app.run(port=8081)
