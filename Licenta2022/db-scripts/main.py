text = """
Bucuresti (1677985)
Constanta (350581)
Iasi (344425)
Timisoara (334115)
Cluj Napoca (328602)
Galati (326141)
Brasov (323736)
Craiova (308895)
Ploiesti (252715)
Braila (234110)
Oradea (222741)
Bacau (205029)
Arad (190114)
Pitesti (179337)
Sibiu (169656)
Targu Mures (164445)
Baia Mare (149205)
Buzau (148087)
Satu Mare (131987)
Botosani (126145)
Piatra Neamt (123360)
Ramnicu Valcea (116914)
Drobeta Turnu Severin (115259)
Suceava (114462)
Focsani (101335)
Targu Jiu (98238)
Targoviste (98117)
Tulcea (97904)
Resita (96918)
Bistrita (87710)
Slatina (85168)
Hunedoara (81337)
Vaslui (80614)
Roman (80328)
Deva (78438)
Barlad (77518)
Calarasi (76952)
Giurgiu (74191)
Alba Iulia (71168)
Zalau (68404)
Sfantu Gheorghe (68359)
Medias (64484)
Turda (61200)
Onesti (58810)
Alexandria (58478)
Slobozia (56048)
Petrosani (52390)
Lugoj (50939)
Tecuci (46825)
Medgidia (46657)
Miercurea Ciuc (46228)
Fagaras (44931)
Pascani (44900)
Sighetu Marmatiei (44185)
Campulung (44125)
Mangalia (43960)
Campina (41554)
Ramnicu Sarat (41405)
Dej (41216)
Odorheiu Secuiesc (39959)
Reghin (39240)
Caracal (39130)
Judetul Suceava (38553)
Rosiori de Vede (37640)
Turnu Magurele (36966)
Sighisoara (36170)
Curtea de Arges (35824)
Fetesti (35374)
Vulcan (34524)
Judetul Timis (34148)
Mioveni (33897)
Dorohoi (33739)
Lupeni (32853)
Falticeni (32807)
Husi (32673)
Caransebes (31985)
Aiud (31894)
Cugir (31877)
Oltenita (31821)
Navodari (31746)
Radauti (31074)
Tarnaveni (31056)
Sacele (30226)
Sebes (29754)
"""

localitati_name_to_id_map = dict()

script_localitati = ""

localitate_index = 1

for line in text.split("\n"):
    nume = line.split("(")[0][:-1]

    if not nume:
        continue

    script_localitati += f"INSERT INTO Localitate (Nume) VALUES ('{nume}');\n"

    localitati_name_to_id_map[nume] = localitate_index

    localitate_index += 1


print(script_localitati)
