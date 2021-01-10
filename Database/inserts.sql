insert into elementType values('fire', 'grass', 'water');
insert into elementType values('water', 'fire', 'grass');
insert into elementType values('grass', 'water', 'fire');
insert into elementType values('normal', '-', '-');

insert into effect values('DoublePower', 'When I kill another monster, double my power for the rest of the game.');
insert into effect values('NegateType', 'Set the targets type to normal for the rest of the game.');
insert into effect values('Overwehlm', 'If I kill the target monster, add my remaining damage to the monster/spell played next round.');
insert into effect values('Silence', 'Remove all effects from the target for the rest of the game.');
insert into effect values('Spellshield', 'I only take half the damage from spells.');
insert into effect values('Undead', 'After Combat, remove me permanently from the battle.');
insert into effect values('OnFire', 'After Combat, I permanently loose 10 attack, if my Type is fire I gain 10 attack instead.');
insert into effect values('SetOnFire', 'Grant an enemy Monster On Fire.');
insert into effect values('-40', 'The next Monster you play gets -20 attack.');
insert into effect values('-20', 'The next Monster you play gets -20 attack.');
insert into effect values('10', 'The next Monster you play gets +15 attack.');
insert into effect values('15', 'The next Monster you play gets +15 attack.');
insert into effect values('20', 'The next Monster you play gets +20 attack.');
insert into effect values('25', 'The next Monster you play gets +25 attack.');
insert into effect values('-', '-');


insert into race values('Human', '-');
insert into race values('Dragon', 'Dragons scare Goblins to death!');
insert into race values('Wizzard', 'Wizzards can control Orks, setting their attack to 0!');
insert into race values('Knight', 'Knight dont know how to swim!');
insert into race values('Elve', 'Elves can evade the attacks of Dragons!');
insert into race values('Kraken', 'Kraken doesnt give a dam about Spell!');
insert into race values('Ork', '-');
insert into race values('Goblin', '-');
insert into race values('-', '-');

-- Spells
insert into card values('Fire Arrow Rain', 20, 'fire', 'SetOnFire', 0, '-');
insert into card values('Hydro Pump', 25, 'water', '-', 0, '-');
insert into card values('Grass Flute', 15, 'grass', '15', 0, '-');
insert into card values('Heroic Charge', 25, 'normal', '-', 0, '-');
insert into card values('For the Nation', 55, 'normal', 'OnFire', 0, '-');
insert into card values('Wait, i get help', 20, 'normal', '15', 0, '-');
insert into card values('Bonk!', 20, 'normal', '-', 0, '-');
insert into card values('Big Bonk!', 30, 'normal', '-', 0, '-');
insert into card values('Vulcan Shower', 50, 'fire', '-', 0, '-');
insert into card values('Sneaky Stabbyn', 30, 'normal', '10', 0, '-');
insert into card values('Cursefire Staff', 10, 'fire', '15,Silence,SetOnFire', 0, '-');
insert into card values('Grass Cannon', 100, 'grass', '-40', 0, '-');
insert into card values('Sneaky Stabbyn', 30, 'normal', '10', 0, '-');
insert into card values('Cursefire Staff', 10, 'fire', '15,Silence,SetOnFire', 0, '-');
insert into card values('Grass Cannon', 100, 'grass', '-40', 0, '');
insert into card values('Etharnal Hunger', 75, 'water', '-', 0, '-');
insert into card values('Life sucking Tentecals', 25, 'water', '25', 0, '-');
insert into card values('Down to the Ground', 60, 'water', 'Silence', 0, '-');

-- Monster
insert into card values('Angry Mob', 20, 'normal', '-', 1, 'Human');
insert into card values('Deep Sea Diver', 20, 'water', '-', 1, 'Human');
insert into card values('Battlesmith', 20, 'fire', '-', 1, 'Human');
insert into card values('Jungle Assassin', 20, 'grass', '-', 1, 'Human');
insert into card values('Body Builder', 30, 'normal', 'Overwehlm', 1, 'Human');

insert into card values('Footsquire', 25, 'normal', '-', 1, 'Knight');
insert into card values('Cavalry', 30, 'normal', '-', 1, 'Knight');
insert into card values('Imperial Fire Archers', 25, 'fire', 'SetOnFire', 1, 'Knight');
insert into card values('Leader Of the Deep', 50, 'water', 'Overwehlm', 1, 'Knight');
insert into card values('Brightsteel Formation', 20, 'fire', 'Spellshield,Silence', 1, 'Knight');
insert into card values('Citrus the Bold', 45, 'grass', 'Overwehlm', 1, 'Knight');

insert into card values('Big bad Orc', 35, 'normal', 'Overwehlm', 1, 'Ork');
insert into card values('Grey Ork', 40, 'normal', 'NegateType', 1, 'Ork');
insert into card values('Magic Troll Hag', 25, 'water', 'Spellshield,Silence', 1, 'Ork');
insert into card values('Azhaeg the Butcher', 50, 'normal', 'Spellshield,Silence,NegateType', 1, 'Ork');
insert into card values('Grimgoer', 50, 'fire', 'Spellshield,DoublePower', 1, 'Ork');

insert into card values('Da Gobbos', 10, 'normal', '25', 1, 'Goblin');
insert into card values('Night Gobbos', 15, 'normal', '25', 1, 'Goblin');
insert into card values('Forest Gobbos', 20, 'grass', '20', 1, 'Goblin');
insert into card values('Gobsnik the Stabba', 25, 'normal', 'NegateType,15', 1, 'Goblin');
insert into card values('Grom the Punsch', 60, 'normal', '-20', 1, 'Goblin');

insert into card values('Kraken of the deep', 30, 'water', '-', 1, 'Kraken');
insert into card values('Skysquid', 15, 'normal', '25,Silence', 1, 'Kraken');
insert into card values('Deep sea Vegan', 25, 'grass', 'SetOnFire', 1, 'Kraken');
insert into card values('Ethernal Kraken King', 75, 'water', 'Undead,Silence', 1, 'Kraken');
insert into card values('Royal Kraken Guard', 30, 'water', 'DoublePower,NegateType', 1, 'Kraken');

insert into card values('Little Fire Witch', 10, 'fire', 'SetOnFire', 1, 'Wizzard');
insert into card values('Expierenced Caster', 35, 'grass', 'Spellshield,DoublePower', 1, 'Wizzard');
insert into card values('Arcane Aqua Sourcerer', 50, 'water', 'Spellshield', 1, 'Wizzard');
insert into card values('Beltasur Geld', 65, 'normal', 'Spellshield,Silence,Overwehlm', 1, 'Wizzard');
insert into card values('Aleriul The Lifemage', 55, 'grass', 'Spellshield,Silence,25', 1, 'Wizzard');

insert into card values('Dragon Baby', 15, 'fire', 'SetOnFire,DoublePower', 1, 'Dragon');
insert into card values('Deep sea Serpent', 50, 'water', 'DoublePower', 1, 'Dragon');
insert into card values('Forest Dragon', 30, 'grass', '25,DoublePower', 1, 'Dragon');
insert into card values('Eragon the First', 75, 'fire', 'SetOnFire,DoublePower', 1, 'Dragon');
insert into card values('Sulafet', 60, 'Normal', 'DoublePower,Spellshield', 1, 'Dragon');

insert into card values('Elven Spearman', 20, 'normal', '-', 1, 'Elve');
insert into card values('Wardancer', 25, 'normal', 'DoublePower,Spellshield', 1, 'Elve');
insert into card values('High Swordmaster', 40, 'normal', 'NegateType', 1, 'Elve');
insert into card values('Aleth Anag', 75, 'normal', 'Silence,Spellshield', 1, 'Elve');
insert into card values('Imruck Dragonborn', 100, 'fire', 'DoublePower,Spellshield', 1, 'Elve');
insert into card values('Kjubie the Phoenixking', 250, 'fire', 'Overwehlm,DoublePower,Spellshield,Silence,NegateType', 1, 'Elve');

insert into mtcguser values ('kjubie', '123', 10, 100);
insert into stackcards values ('kjubie', 'Citrus the Bold');
insert into stackcards values ('kjubie', 'Imperial Fire Archers');
insert into stackcards values ('kjubie', 'Leader Of the Deep');
insert into stackcards values ('kjubie', 'Jungle Assassin');
insert into stackcards values ('kjubie', 'Fire Arrow Rain');
insert into deckcards values ('kjubie', 'Citrus the Bold');
insert into deckcards values ('kjubie', 'Imperial Fire Archers');
insert into deckcards values ('kjubie', 'Leader Of the Deep');
insert into deckcards values ('kjubie', 'Fire Arrow Rain');

insert into mtcguser values ('1z3ro37', '123', 10, 100);