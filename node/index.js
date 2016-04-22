

var express = require('express');
var app = express();
var bl = require('bl')

const crypto = require('crypto');
const hmac = crypto.createHmac('sha1', 'secret');

var KEY = "secret";

app.post('/', function (req, res) {
  var EXPECTED = req.get('x-hub-signature');
  req.pipe(bl(function (err, data) {
    if (err) {
      return hasError(err.message)
    }
    var obj = JSON.parse(data.toString())
    console.log("===============================================================");
    console.log(data.toString());
    console.log("===============================================================");

    if (!EXPECTED){
      console.log("Not signed. Not calculating");
    }
    else{
      var CALCULATED = 'sha1=' + crypto.createHmac('sha1', KEY).update(data).digest('hex');
      console.log("Expected  : " + EXPECTED);
      console.log("Calculated: " + CALCULATED);
      console.log("Match?    : " + (CALCULATED == EXPECTED));
    }
    console.log("Topic Recieved: " + obj["topic"]);

  }));

  res.sendStatus(200);
});

app.listen(3000, function () {
  console.log('Example app listening on port 3000!');
});
