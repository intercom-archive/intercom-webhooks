import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import spark.Request;
import spark.Response;
import spark.Route;

import static spark.Spark.*;
import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;



public class Main {
    public static void main(String[] args) {


        post(new Route("/") {
            @Override
            public Object handle(Request request, Response response) {
                String secret = "secret";

                String message = request.body();
                System.out.println("===============================================================");
                System.out.println(message);
                System.out.println("===============================================================");


                String signature = request.headers("X-Hub-Signature");
                signature = (signature == null ? "" : signature.trim());
                if(signature.equals("")){
                    System.out.println("Not signed. Not calculating");
                }
                else{
                    try {

                        Mac HMAC = Mac.getInstance("HmacSHA1");
                        SecretKeySpec secret_key = new SecretKeySpec(secret.getBytes(), "HmacSHA256");
                        HMAC.init(secret_key);

                        byte[] hash = (HMAC.doFinal(message.getBytes()));
                        StringBuffer result = new StringBuffer();
                        for (byte b : hash) {
                            result.append(String.format("%02X", b));
                        }
                        String calculated = "sha1=" + result.toString();
                        System.out.println("Expected  : " + signature);
                        System.out.println("Calculated: " + calculated);
                        System.out.println("Match?    : " + calculated.equalsIgnoreCase(signature));
                    }
                    catch (Exception e){
                        System.out.println("Error: " + e.getMessage());
                    }
                }
                JsonObject output = new JsonParser().parse(message).getAsJsonObject();
                System.out.println("Topic Recieved: " + output.get("topic").getAsString());
                response.status(200);
                return "OK";
            }
        });
    }
}
