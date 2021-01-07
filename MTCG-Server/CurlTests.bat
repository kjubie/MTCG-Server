curl -X POST -H "Content-Type: text/plain" --data "Hallo Text!" http://127.0.0.1:25575/
curl -X POST -H "Content-Type: text/plain" --data "Text Nachricht" http://127.0.0.1:25575/
curl -X POST -H "Content-Type: text/plain" --data "Keine Ahnung" http://127.0.0.1:25575/
curl http://127.0.0.1:25575/messages
curl http://127.0.0.1:25575/message/0
curl http://127.0.0.1:25575/message/2
curl http://127.0.0.1:25575/message/32
curl http://127.0.0.1:25575/dsiusdh
curl -X PUT -H "Content-Type: text/plain" --data "Jetzt habe ich Ahnung" http://127.0.0.1:25575/message/2
curl http://127.0.0.1:25575/message/2
curl -X DELETE http://127.0.0.1:25575/message/2
curl http://127.0.0.1:25575/messages
PAUSE