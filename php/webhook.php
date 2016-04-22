<?php

$stdout = fopen('php://stdout', 'w');
function readInput(){
  $inputContent = '';
  $input = fopen('php://input' , 'rb');
  while (!feof($input)) {
      $inputContent .= fread($input, 4096);
  }
  fclose($input);
  return $inputContent;
}

function printOutput($msg){
  global $stdout;
  fwrite($stdout, $msg . "\n");
}

function verify_signature($payload_body){
  $ALGO = 'sha1';
  $SECRET = "secret";
  $expected = $_SERVER['HTTP_X_HUB_SIGNATURE'];
  if(empty($expected)){
    printOutput("Not signed. Not calculating");
  }
  else{
    $calculated = "sha1=" . hash_hmac($ALGO, $payload_body, $SECRET, false);
    printOutput("Expected  : " . $expected);
    printOutput("Calculated: " . $calculated);
    printOutput("Match?    : " . ($expected === $calculated ? "true" : "false"));
  }
}

$DATA = readInput();
printOutput("===============================================================");
printOutput($DATA);
printOutput("===============================================================");
verify_signature($DATA);
$push = json_decode($DATA);
printOutput("Topic Recieved: " . $push->topic);
