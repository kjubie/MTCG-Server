MTCG Doku:

Dokumentation zum Code befindet sich in form von Kommentaren im Code!

Design (ClassDiagram.cd gibts auch):
 
	HTTPListener: Es gibt ZWEI unterschiedliche Socket Listener:
		1. Socket für alles was nicht Battle ist:
			- Class: Listener
			- Port: 25575
			- RequestContext: Speichert den HTTP Header und Body.
			- RequestHandler: Führt das gewollte Request aus.

		2. Socket für die Battles:
			- Class: BattleConnection
			- Port 25576
			- RequestContext: Speichert den HTTP Header und Body.
			- BattleHandler: Führt das Battle durch.

	User:
		Hat seine Karten in einem Dictionary 'collection' gespeichert.
		Hat seine Deckkarten in einem Dictionary 'deck' gespeichert.
		Hat sein token in 'token' gespeichert.
		Hat seinen Namen, Elo, Credits, und Wins/Losses(Ist ein Zusatzfeature) gespeichert.

	Card:
		Card ist eine abstract class mit einem Namen, Schaden, Element Typ und einem effects Dictionary (Effekte kommen gleich).

		SpellCard:
			Konkretes Objekt einer Card

		MonsterCard:
			Konkretes Objekt einer Card, mit zusätzlich einer Rasse (Kommen auch gleich).

	ElementType:
		Ein Elemen Typ ist der Typ einer Karte der gegen andere Typen effektiv oder ineffektiv ist
		Typen:
			Feuer: effektiv: Grass, ineffektiv: Wasser
			Wasser: effektiv: Feuer, ineffektiv: Grass
			Grass: effektiv: Wasser, ineffektiv: Feuer
			Normal: effektiv: -, ineffektiv: - (Ist ein Zusatzfeature)

	Race:
		Ist eine abstract class mit einem Namen, Beschreibungstext und einer abstract function 'DoRaceEffect'.
		'DoRaceEffect' wird ausgeführt wenn eine Karte gespielt wird und bekommt die Karte als Parameter die gegen sie gespielt wurde und errechet somit
		wieviel Schaden die gegnerische Karte ihr machen würde.

		Z.B. Goblins können keine Drachen angreifen:
			Drache.DoRaceEffect(Goblin) setzt den Schaden von Goblin auf 0.

		Rassen:
			- Dragon: Schüchtert Goblin ein.
			- Elve: Weichen Drachen aus.
			- Goblin: Angst vor Drachen.
			- Human: Neutral (Ist ein Zusatzfeature).
			- Knight: Verliert immer gegen Wasserspells.
			- Kraken: Immume gegen Spells.
			- Ork: Wird von Wizzard kontrolliert.
			- Wizzard: Kann Orks kontrollieren.

	Effect (Ist ein Zusatzfeature): 
		Ist eine abstract class mit einem Namen, Beschreibungstext und zwei abstract functions 'DoBeforeEffect' und 'DoAfterEffect'.
		'DoBeforeEffect' wird ausgeführt bevor der Schaden verrechnet wird und 'DoAfterEffect' nachdem der Schaden verrechnet wurde.

		Effekte:
			- OnFire: Nach jeder Runde verliert die Karte 15 Schaden, außer es ist eine Karte vom Typ Feuer, dann bekommt sie 15 Schaden dazu.
			- SetOnFire: Gibt der gegnerischen Karte den Effekt OnFire.
			- Overwehlm: Der überschüssige Schaden wird auf die Karte die nächste Runde gespielt wird, draufgerechnet.
			- Buff: Buff hat einen buffValue (Positiv oder Negativ) dieser buffValue wird auf die Karte die nächste Runde gespielt wird, draufgerechnet.
			- Silence: Entfernt alle Effekte von der gegnerischen Karte bis zum Ende des Kampfes.
			- DoublePower: Wenn diese Karte einen Gegner besiegt, dann wird ihr Schaden verdoppelt.
			- Spellshield: Diese Karte erhält nur halbsoviel Schaden von Spells.
			- Undead: Diese Karte verschwindet am eine einer Runde für den restlichen Kampf, egal ob gewonnen, verloren oder unentschieden.
			- NegateType: Setzt den Typ von der gegnerischen Karte, auf 'Normal', bis zum Ende des Kampfes.

	Battle:
		Jeder user hat ein Deck mit 4 Karten. Am Anfang der Runde zieht jeder Spieler zwei Handkarten (außer er hat nur mehr eine) 
		und kann sich aussuchen welche er davon spielt (Ist ein Zusatzfeature).
		Wenn beide Spieler ihre Karte ausgesucht haben, wird der Schaden, Effekte, Typ und Rassen verrechnet.
		Der Spieler der gewinnt bekommt beide Karten in sein Deck.
		Wenn ein Spieler keine Karte mehr ziehen kann, verliert er.

		(Dadurch das Battles Userinteraktion brauchen und unterschiedlich lang dauern können, ist das mit einem CURL Script schwierig zu Testen. Werde ich dann manuell herzeigen)
	
		Der Gewinner bekommt:
			+ 5 Elo
			+ 1 Credit (Ist ein Zusatzfeature)
			+ 1 Win

		Der Verlierer bekommt:
			- 10 Elo
			+ 1 Loss

	Karten erhalten:
		Jeder User hat am Anfang 10 Credits (Kann man durch gewonnene Kämpfe auch bekommen) und kann sich um 2 Credits ein Pack kaufen.
		Ein Pack enthält 4 ZUFÄLLIGE Karten die der User noch nicht hat (man kann keine Karte doppelt haben)
		(Ist auch mit CURL schwer zu Testen, diese zuffäligen Karten ins Deck hinzuzufügen. Werde ich dann manuell herzeigen).

	Trading:
		Es gibt zwar ein feature das Karten nicht gleichzeitig im Deck und im Store sein können, es gibt jedoch keinen Store und somit kein Trading.

Unit Tests:
	- BattleTests
	- TypeTests
	- DBTests
	- AuthTests
	- EffectsTests

Time spent:
	Ca. 45h.

Git:
	https://github.com/kjubie/SWE-MTCG
	 
		
		
	