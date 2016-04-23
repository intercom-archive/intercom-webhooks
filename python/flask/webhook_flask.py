# install dependencies
#    pip install -r requirements.txt
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
    try:
        KEY = str("secret")

        DATA = request.get_data()
        print ("===============================================================");
        print (DATA.decode());
        print ("===============================================================");
        EXPECTED = ""
        if not request.headers.get('X-Hub-Signature') is None :
            EXPECTED = str(request.headers.get('X-Hub-Signature'))
        if not EXPECTED :
            print("Not signed. Not calculating");
        else :
            calculated = hmac.new(KEY.encode("ascii"), DATA, hashlib.sha1).hexdigest()
            calculated = "sha1=" + (calculated)

            print("Expected  : " + EXPECTED);
            print("Calculated: " + calculated);
            print("Match?    : " + str(calculated == EXPECTED));
        pass
        output = json.loads(DATA.decode())
        print("Topic Recieved: " + output["topic"]);
        pass
    except Exception as e:
        print(e)
        raise
    return("OK")

if __name__ == '__main__':
    app.run()
