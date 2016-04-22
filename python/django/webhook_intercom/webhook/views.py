from django.views.decorators.csrf import csrf_exempt

import hashlib
import hmac
import json

from django.http import HttpResponse

@csrf_exempt
def index(request):
    KEY = "secret"
    if request.method == "POST" :
        EXPECTED = ""
        DATA = request.body
        print "===============================================================";
        print DATA;
        print "===============================================================";
        if not request.META.get('HTTP_X_HUB_SIGNATURE') is None :
            EXPECTED = str(request.META.get('HTTP_X_HUB_SIGNATURE'))
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

    return HttpResponse()
