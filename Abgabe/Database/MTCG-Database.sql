create table if not exists mtcguser (
	username varchar(24) primary key,
	password varchar(64) not null,
	credits int,
	elo int
);

create table if not exists elementType (
	elementName varchar(24) primary key,
	strong varchar(24),
	weak varchar(24)
);

create table if not exists effect (
	effectName varchar(24) primary key,
	effectText varchar(128)
);

create table if not exists race (
	raceName varchar(24) primary key,
	raceText varchar(128)
);

create table if not exists card (
	cardName varchar(24) primary key,
	damage int not null,
	elementName varchar(24),
	effectName varchar(64),
	spellOrMonster int, -- 0 = spell, 1 = monster
	raceName varchar(24),
	foreign key (elementName) references elementType(elementName),
	foreign key (raceName) references race(raceName)
);

create table if not exists stackCards (
	userName varchar(24),
	cardName varchar(24) unique,
	foreign key (userName) references mtcguser(userName),
	foreign key (cardName) references card(cardName)
);

create table if not exists deckCards (
	username varchar(24),
	cardName varchar(24) unique,
	foreign key (username) references mtcguser(username),
	foreign key (cardName) references stackCards(cardName)
);