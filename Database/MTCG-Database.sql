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

create table if not exists stack (
	stackId serial primary key,
	username varchar(24),
	foreign key (username) references mtcguser(username)
);

create table if not exists cardInStack (
	stackId serial,
	cardName varchar(24),
	primary key (stackId, cardName),
	foreign key (stackId) references stack(stackId),
	foreign key (cardName) references card(cardName)
);

create table if not exists deck (
	username varchar(24) primary key,
	foreign key (username) references mtcguser(username)
);

create table if not exists cardInDeck (
	username varchar(24),
	cardName varchar(24),
	primary key (username, cardName),
	foreign key (username) references deck(username),
	foreign key (cardName) references card(cardName)
);