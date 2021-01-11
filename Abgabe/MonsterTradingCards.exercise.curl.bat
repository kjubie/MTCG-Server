@echo off

REM --------------------------------------------------
REM Monster Trading Cards Game
REM --------------------------------------------------
title Monster Trading Cards Game
echo CURL Testing for Monster Trading Cards Game
echo.

REM --------------------------------------------------
echo 1) Create Users (Registration)
REM Create User
curl -X POST http://localhost:25575/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:25575/users --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:25575/users --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.

echo should fail:
curl -X POST http://localhost:25575/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:25575/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo. 
echo.

REM --------------------------------------------------
echo 2) Login Users
curl -X POST http://localhost:25575/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:25575/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:25575/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.

echo should fail:
curl -X POST http://localhost:25575/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo.
echo.

REM --------------------------------------------------
echo 4) acquire packages kienboec
curl -X POST http://localhost:25575/transactions/packages --header "Content-Type: application/json" --header "Authorization: kienboec-mtcgToken" -d ""
echo.
curl -X POST http://localhost:25575/transactions/packages --header "Content-Type: application/json" --header "Authorization: kienboec-mtcgToken" -d ""
echo.
curl -X POST http://localhost:25575/transactions/packages --header "Content-Type: application/json" --header "Authorization: kienboec-mtcgToken" -d ""
echo.
curl -X POST http://localhost:25575/transactions/packages --header "Content-Type: application/json" --header "Authorization: kienboec-mtcgToken" -d ""
echo.
echo should fail (no money):
curl -X POST http://localhost:25575/transactions/packages --header "Content-Type: application/json" --header "Authorization: kienboec-mtcgToken" -d ""
echo.
echo.

REM --------------------------------------------------
echo 5) acquire packages altenhof
curl -X POST http://localhost:25575/transactions/packages --header "Content-Type: application/json" --header "Authorization: altenhof-mtcgToken" -d ""
echo.
curl -X POST http://localhost:25575/transactions/packages --header "Content-Type: application/json" --header "Authorization: altenhof-mtcgToken" -d ""
echo.
echo.

REM --------------------------------------------------
echo 8) show all acquired cards kienboec
curl -X GET http://localhost:25575/cards --header "Authorization: kienboec-mtcgToken"
echo should fail (no token)
curl -X GET http://localhost:25575/cards 
echo.
echo.

REM --------------------------------------------------
echo 9) show all acquired cards altenhof
curl -X GET http://localhost:25575/cards --header "Authorization: altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 10) show unconfigured deck
curl -X GET http://localhost:25575/deck --header "Authorization: kienboec-mtcgToken"
echo.
curl -X GET http://localhost:25575/deck --header "Authorization: altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 11) configure deck
curl -X PUT http://localhost:25575/deck --header "Content-Type: text/plain" --header "Authorization: kienboec-mtcgToken" -d "Citrus the Bold,Imperial Fire Archers,Jungle Assassin,Fire Arrow Rain"
echo.
curl -X GET http://localhost:25575/deck --header "Authorization: kienboec-mtcgToken"
echo.
curl -X PUT http://localhost:25575/deck --header "Content-Type: application/json" --header "Authorization: altenhof-mtcgToken" -d "[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]"
echo.
curl -X GET http://localhost:25575/deck --header "Authorization: altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 12) show configured deck 
curl -X GET http://localhost:25575/deck --header "Authorization: kienboec-mtcgToken"
echo.
curl -X GET http://localhost:25575/deck --header "Authorization: altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 14) edit user data
echo.
curl -X GET http://localhost:25575/users/kienboec --header "Authorization: kienboec-mtcgToken"
echo.
curl -X GET http://localhost:25575/users/altenhof --header "Authorization: altenhof-mtcgToken"
echo.
curl -X PUT http://localhost:25575/users/kienboec --header "Content-Type: application/json" --header "Authorization: kienboec-mtcgToken" -d "{\"Name\": \"Kienboeck\"}"
echo.
curl -X PUT http://localhost:25575/users/altenhof --header "Content-Type: application/json" --header "Authorization: altenhof-mtcgToken" -d "{\"Name\": \"Altenhofer\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
echo.
curl -X GET http://localhost:25575/users/kienboec --header "Authorization: kienboec-mtcgToken"
echo.
curl -X GET http://localhost:25575/users/altenhof --header "Authorization: altenhof-mtcgToken"
echo.
echo.
echo should fail:
curl -X PUT http://localhost:25575/users/kienboec --header "Content-Type: application/json" --header "Authorization: altenhof-mtcgToken" -d "{\"Name\": \"Hoax\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
echo.
curl -X PUT http://localhost:25575/users/altenhof --header "Content-Type: application/json" --header "Authorization: kienboec-mtcgToken" -d "{\"Name\": \"Hoax\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
echo.
curl -X GET http://localhost:25575/users/someGuy  --header "Authorization: kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 15) stats
curl -X GET http://localhost:25575/stats --header "Authorization: kienboec-mtcgToken"
echo.
curl -X GET http://localhost:25575/stats --header "Authorization: altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 16) scoreboard
curl -X GET http://localhost:25575/score --header "Authorization: kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 18) Stats 
echo kienboec
curl -X GET http://localhost:25575/stats --header "Authorization: kienboec-mtcgToken"
echo.
echo altenhof
curl -X GET http://localhost:25575/stats --header "Authorization: altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 19) scoreboard
curl -X GET http://localhost:25575/score --header "Authorization: kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo end...

REM this is approx a sleep 
ping localhost -n 100 >NUL 2>NUL
@echo on
