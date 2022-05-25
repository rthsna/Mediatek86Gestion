-- phpMyAdmin SQL Dump
-- version 5.1.3
-- https://www.phpmyadmin.net/
--
-- Hôte : localhost
-- Généré le : jeu. 21 avr. 2022 à 22:41
-- Version du serveur : 10.2.38-MariaDB
-- Version de PHP : 7.2.34

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `mediatek86`
--

DELIMITER $$
--
-- Procédures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `whoExpire` ()   BEGIN
	SELECT id
	FROM  abonnement
	WHERE  DATE_ADD(NOW(), INTERVAL 1 MONTH) > dateFinAbonnement ;
	
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `abonnement`
--

CREATE TABLE `abonnement` (
  `id` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `dateFinAbonnement` date DEFAULT NULL,
  `idRevue` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `abonnement`
--

INSERT INTO `abonnement` (`id`, `dateFinAbonnement`, `idRevue`) VALUES
('51', '2022-04-22', '10002'),
('52', '2022-04-24', '10002'),
('53', '2022-04-07', '10007'),
('54', '2022-04-09', '10002'),
('62', '2022-04-08', '10003'),
('65', '2022-04-09', '10004'),
('66', '2022-04-10', '10004'),
('67', '2022-04-17', '10004'),
('69', '2022-04-13', '10002'),
('70', '2022-04-30', '10002'),
('71', '2022-04-30', '10002'),
('72', '2022-04-30', '10002'),
('73', '2022-04-30', '10002'),
('74', '2022-04-30', '10002'),
('75', '2022-04-30', '10002'),
('76', '2022-04-30', '10002'),
('77', '2022-05-03', '10002'),
('78', '2022-05-03', '10002'),
('79', '2022-05-06', '10002'),
('81', '2022-04-09', '10002');

--
-- Déclencheurs `abonnement`
--
DELIMITER $$
CREATE TRIGGER `controleE` BEFORE INSERT ON `abonnement` FOR EACH ROW BEGIN
	DECLARE nb_count INT;
	SELECT count(*) into nb_count
	FROM commandedocument
	Where commandedocument.id = New.id;
	
	if nb_count > 0 THEN
	SIGNAL SQLSTATE "45000"
 	SET MESSAGE_TEXT = "impossible \td'ajouter une commande car la contrainte de partition n'est pas respecté " ;
    END IF;

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `commande`
--

CREATE TABLE `commande` (
  `id` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `dateCommande` date DEFAULT NULL,
  `montant` double DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `commande`
--

INSERT INTO `commande` (`id`, `dateCommande`, `montant`) VALUES
('100', '2022-04-11', 232),
('101', '2022-04-11', 321),
('102', '2022-04-11', 2332),
('103', '2022-04-11', 232),
('104', '2022-04-11', 343),
('105', '2022-04-11', 232),
('106', '2022-04-11', 231),
('107', '2022-04-11', 237),
('108', '2022-04-11', 234),
('109', '2022-04-11', 324),
('110', '2022-04-11', 543),
('111', '2022-04-12', 323),
('112', '2022-04-18', 211),
('2', '2022-04-04', 450),
('20', '2022-04-05', 235),
('21', '2022-04-05', 346),
('24', '2022-04-05', 323),
('27', '2022-04-06', 231),
('3', '2022-04-04', 453),
('37', '2022-04-06', 231),
('38', '2022-04-06', 2343),
('41', '2022-04-06', 2332),
('43', '2022-04-06', 342),
('47', '2022-04-06', 234),
('50', '2022-04-07', 12),
('51', '2022-04-07', 231),
('52', '2022-04-07', 100),
('53', '2022-04-07', 120),
('54', '2022-04-07', 123),
('56', '2022-04-07', 123),
('62', '2022-04-07', 231),
('64', '2022-04-07', 123),
('65', '2022-04-07', 234),
('66', '2022-04-07', 234),
('67', '2022-04-07', 12),
('69', '2022-04-08', 232),
('70', '2022-04-10', 450),
('71', '2022-04-10', 450),
('72', '2022-04-10', 450),
('73', '2022-04-10', 450),
('74', '2022-04-10', 450),
('75', '2022-04-10', 450),
('76', '2022-04-10', 450),
('77', '2022-04-10', 450),
('78', '2022-04-10', 450),
('79', '2022-04-10', 4323),
('8', '2022-04-05', 323),
('80', '2022-04-11', 123),
('81', '2022-04-11', 123),
('82', '2022-04-11', 231),
('83', '2022-04-11', 231),
('84', '2022-04-11', 231),
('85', '2022-04-11', 233),
('86', '2022-04-11', 231),
('87', '2022-04-11', 100),
('88', '2022-04-11', 211),
('89', '2022-04-11', 2111),
('90', '2022-04-11', 211),
('91', '2022-04-11', 232),
('92', '2022-04-11', 232),
('93', '2022-04-11', 212),
('94', '2022-04-11', 213),
('95', '2022-04-11', 231),
('96', '2022-04-11', 231),
('97', '2022-04-11', 343),
('98', '2022-04-11', 2321),
('99', '2022-04-11', 321);

-- --------------------------------------------------------

--
-- Structure de la table `commandedocument`
--

CREATE TABLE `commandedocument` (
  `id` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `nbExemplaire` int(11) DEFAULT NULL,
  `idLivreDvd` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idsuivi` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `commandedocument`
--

INSERT INTO `commandedocument` (`id`, `nbExemplaire`, `idLivreDvd`, `idsuivi`) VALUES
('100', 4, '00004', 'LI'),
('101', 2, '00007', 'LI'),
('102', 3, '00007', 'LI'),
('103', 2, '00007', 'LI'),
('104', 2, '00008', 'LI'),
('105', 4, '00008', 'LI'),
('106', 4, '00008', 'LI'),
('107', 2, '00008', 'LI'),
('108', 4, '00008', 'LI'),
('109', 4, '00007', 'LI'),
('110', 3, '00007', 'LI'),
('111', 3, '00004', 'LI'),
('112', 2, '00010', 'EC'),
('2', 4, '00007', 'LI'),
('24', 121, '00017', 'LI'),
('27', 4, '00017', 'LI'),
('3', 3, '00003', 'EC'),
('37', 2, '00004', 'LI'),
('38', 3, '00004', 'LI'),
('41', 3, '20003', 'LI'),
('43', 3, '20004', 'EC'),
('47', 2, '20003', 'LI'),
('56', 2, '00017', 'LI'),
('64', 1, '20003', 'EC'),
('8', 4, '00004', 'LI'),
('80', 2, '00017', 'EC'),
('82', 2, '00004', 'LI'),
('83', 3, '00004', 'EC'),
('84', 2, '00004', 'EC'),
('85', 2, '00004', 'EC'),
('86', 3, '00004', 'EC'),
('87', 5, '00004', 'EC'),
('88', 2, '00004', 'EC'),
('89', 2, '00004', 'EC'),
('90', 8, '00004', 'EC'),
('91', 2, '00004', 'EC'),
('92', 4, '00004', 'EC'),
('93', 1, '00004', 'EC'),
('94', 4, '00004', 'LI'),
('95', 5, '00004', 'LI'),
('96', 4, '00004', 'EC'),
('97', 3, '00004', 'EC'),
('98', 2, '00004', 'EC'),
('99', 4, '00004', 'EC');

--
-- Déclencheurs `commandedocument`
--
DELIMITER $$
CREATE TRIGGER `addExemplaire` BEFORE UPDATE ON `commandedocument` FOR EACH ROW BEGIN
	DECLARE nb_copie INT;
	DECLARE numexemplaire INT;
	DECLARE dt_Achat DATETIME;	
	Declare nb_count INT;
	SET nb_copie = 0;
    SET nb_count = 0;
	if New.idsuivi = "LI" THEN
    
    
	SELECT count(*) into nb_copie 
	FROM exemplaire 
	where exemplaire.id = NEW.idLivreDvd;
	SET numexemplaire = nb_copie + CAST(OLD.idLivreDvd as UNSIGNED INTEGER) + 1	;

	WHILE(nb_count < OLD.nbExemplaire) DO

	Select dateCommande into dt_Achat
	FROM commande
	WHERE commande.id = OLD.id;
	
	INSERT INTO exemplaire (id, numero, dateAchat ,photo, idEtat)
        VALUES(OLD.idLivreDvd, numexemplaire, dt_Achat," ", "00001")  ;   
	SET nb_count = nb_count +1;
	SET numexemplaire = numexemplaire + 1;	
	END WHILE;
	
	   
END IF;

END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `controle` BEFORE INSERT ON `commandedocument` FOR EACH ROW BEGIN
	DECLARE nb_count INT;
	SELECT count(*) into nb_count
	FROM abonnement
	Where abonnement.id = New.id;
	
	if nb_count > 0 THEN
	SIGNAL SQLSTATE "45000"
 	SET MESSAGE_TEXT = "impossible \td'ajouter une commande car la contrainte de partition n'est pas respecté " ;
    END IF;

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `document`
--

CREATE TABLE `document` (
  `id` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `titre` varchar(60) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `image` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `idRayon` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idPublic` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idGenre` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `document`
--

INSERT INTO `document` (`id`, `titre`, `image`, `idRayon`, `idPublic`, `idGenre`) VALUES
('00001', 'Quand sort la recluse', '', 'LV003', '00002', '10014'),
('00002', 'Un pays à l\'aube', '', 'LV001', '00002', '10004'),
('00003', 'Et je danse aussi', '', 'LV002', '00003', '10013'),
('00004', 'L\'armée furieuse', '', 'LV003', '00002', '10014'),
('00005', 'Les anonymes', '', 'LV001', '00002', '10014'),
('00006', 'La marque jaune', '', 'BD001', '00003', '10001'),
('00007', 'Dans les coulisses du musée', '', 'LV001', '00003', '10006'),
('00008', 'Histoire du juif errant', '', 'LV002', '00002', '10006'),
('00009', 'Pars vite et reviens tard', '', 'LV003', '00002', '10014'),
('00010', 'Le vestibule des causes perdues', '', 'LV001', '00002', '10006'),
('00011', 'L\'île des oubliés', '', 'LV002', '00003', '10006'),
('00012', 'La souris bleue', '', 'LV002', '00003', '10006'),
('00013', 'Sacré Pêre Noël', '', 'JN001', '00001', '10001'),
('00014', 'Mauvaise étoile', '', 'LV003', '00003', '10014'),
('00015', 'La confrérie des téméraires', '', 'JN002', '00004', '10014'),
('00016', 'Le butin du requin', '', 'JN002', '00004', '10014'),
('00017', 'Catastrophes au Brésil', '', 'JN002', '00004', '10014'),
('00018', 'Le Routard - Maroc', '', 'DV005', '00003', '10011'),
('00019', 'Guide Vert - Iles Canaries', '', 'DV005', '00003', '10011'),
('00020', 'Guide Vert - Irlande', '', 'DV005', '00003', '10011'),
('00021', 'Les déferlantes', '', 'LV002', '00002', '10006'),
('00022', 'Une part de Ciel', '', 'LV002', '00002', '10006'),
('00023', 'Le secret du janissaire', '', 'BD001', '00002', '10001'),
('00024', 'Pavillon noir', '', 'BD001', '00002', '10001'),
('00025', 'L\'archipel du danger', '', 'BD001', '00002', '10001'),
('00026', 'La planète des singes', '', 'LV002', '00003', '10002'),
('10001', 'Arts Magazine', '', 'PR002', '00002', '10016'),
('10002', 'Alternatives Economiques', '', 'PR002', '00002', '10015'),
('10003', 'Challenges', '', 'PR002', '00002', '10015'),
('10004', 'Rock and Folk', '', 'PR002', '00002', '10016'),
('10005', 'Les Echos', '', 'PR001', '00002', '10015'),
('10006', 'Le Monde', '', 'PR001', '00002', '10018'),
('10007', 'Telerama', '', 'PR002', '00002', '10016'),
('10008', 'L\'Obs', '', 'PR002', '00002', '10018'),
('10009', 'L\'Equipe', '', 'PR001', '00002', '10017'),
('10010', 'L\'Equipe Magazine', '', 'PR002', '00002', '10017'),
('10011', 'Geo', '', 'PR002', '00003', '10016'),
('20001', 'Star Wars 5 L\'empire contre attaque', '', 'DF001', '00003', '10002'),
('20002', 'Le seigneur des anneaux : la communauté de l\'anneau', '', 'DF001', '00003', '10019'),
('20003', 'Jurassic Park', '', 'DF001', '00003', '10002'),
('20004', 'Matrix', '', 'DF001', '00003', '10002');

-- --------------------------------------------------------

--
-- Structure de la table `dvd`
--

CREATE TABLE `dvd` (
  `id` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `synopsis` text COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `realisateur` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `duree` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `dvd`
--

INSERT INTO `dvd` (`id`, `synopsis`, `realisateur`, `duree`) VALUES
('20001', 'Luc est entraîné par Yoda pendant que Han et Leia tentent de se cacher dans la cité des nuages.', 'George Lucas', 124),
('20002', 'L\'anneau unique, forgé par Sauron, est porté par Fraudon qui l\'amène à Foncombe. De là, des représentants de peuples différents vont s\'unir pour aider Fraudon à amener l\'anneau à la montagne du Destin.', 'Peter Jackson', 228),
('20003', 'Un milliardaire et des généticiens créent des dinosaures à partir de clonage.', 'Steven Spielberg', 128),
('20004', 'Un informaticien réalise que le monde dans lequel il vit est une simulation gérée par des machines.', 'Les Wachowski', 136);

-- --------------------------------------------------------

--
-- Structure de la table `etat`
--

CREATE TABLE `etat` (
  `id` char(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `libelle` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `etat`
--

INSERT INTO `etat` (`id`, `libelle`) VALUES
('00001', 'neuf'),
('00002', 'usagé'),
('00003', 'détérioré'),
('00004', 'inutilisable');

-- --------------------------------------------------------

--
-- Structure de la table `exemplaire`
--

CREATE TABLE `exemplaire` (
  `id` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `numero` int(11) NOT NULL,
  `dateAchat` date DEFAULT NULL,
  `photo` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idEtat` char(5) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `exemplaire`
--

INSERT INTO `exemplaire` (`id`, `numero`, `dateAchat`, `photo`, `idEtat`) VALUES
('00004', 5, '2022-04-12', ' ', '00001'),
('00004', 6, '2022-04-12', ' ', '00001'),
('00004', 7, '2022-04-12', ' ', '00001'),
('00007', 8, '2022-04-11', ' ', '00001'),
('00007', 9, '2022-04-11', ' ', '00001'),
('00007', 10, '2022-04-11', ' ', '00001'),
('00007', 11, '2022-04-11', ' ', '00001'),
('00007', 12, '2022-04-11', ' ', '00001'),
('00007', 13, '2022-04-11', ' ', '00001'),
('00007', 14, '2022-04-11', ' ', '00001'),
('00008', 9, '2022-04-11', ' ', '00001'),
('00008', 10, '2022-04-11', ' ', '00001'),
('00008', 11, '2022-04-11', ' ', '00001'),
('00008', 12, '2022-04-11', ' ', '00001'),
('00008', 13, '2022-04-11', ' ', '00001'),
('00008', 14, '2022-04-11', ' ', '00001'),
('00008', 15, '2022-04-11', ' ', '00001'),
('00008', 16, '2022-04-11', ' ', '00001'),
('10002', 418, '2021-12-01', '', '00001'),
('10007', 3237, '2021-11-23', '', '00001'),
('10007', 3238, '2021-11-30', '', '00001'),
('10007', 3239, '2021-12-07', '', '00001'),
('10007', 3240, '2021-12-21', '', '00001'),
('10007', 10009, '2022-04-05', '', '00001'),
('10011', 506, '2021-04-01', '', '00001'),
('10011', 507, '2021-05-03', '', '00001'),
('10011', 508, '2021-06-05', '', '00001'),
('10011', 509, '2021-07-01', '', '00001'),
('10011', 510, '2021-08-04', '', '00001'),
('10011', 511, '2021-09-01', '', '00001'),
('10011', 512, '2021-10-06', '', '00001'),
('10011', 513, '2021-11-01', '', '00001'),
('10011', 514, '2021-12-01', '', '00001');

-- --------------------------------------------------------

--
-- Structure de la table `genre`
--

CREATE TABLE `genre` (
  `id` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `libelle` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `genre`
--

INSERT INTO `genre` (`id`, `libelle`) VALUES
('10000', 'Humour'),
('10001', 'Bande dessinée'),
('10002', 'Science Fiction'),
('10003', 'Biographie'),
('10004', 'Historique'),
('10006', 'Roman'),
('10007', 'Aventures'),
('10008', 'Essai'),
('10009', 'Documentaire'),
('10010', 'Technique'),
('10011', 'Voyages'),
('10012', 'Drame'),
('10013', 'Comédie'),
('10014', 'Policier'),
('10015', 'Presse Economique'),
('10016', 'Presse Culturelle'),
('10017', 'Presse sportive'),
('10018', 'Actualités'),
('10019', 'Fantazy');

-- --------------------------------------------------------

--
-- Structure de la table `livre`
--

CREATE TABLE `livre` (
  `id` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ISBN` varchar(13) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `auteur` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `collection` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `livre`
--

INSERT INTO `livre` (`id`, `ISBN`, `auteur`, `collection`) VALUES
('00001', '1234569877896', 'Fred Vargas', 'Commissaire Adamsberg'),
('00002', '1236547896541', 'Dennis Lehanne', ''),
('00003', '6541236987410', 'Anne-Laure Bondoux', ''),
('00004', '3214569874123', 'Fred Vargas', 'Commissaire Adamsberg'),
('00005', '3214563214563', 'RJ Ellory', ''),
('00006', '3213213211232', 'Edgar P. Jacobs', 'Blake et Mortimer'),
('00007', '6541236987541', 'Kate Atkinson', ''),
('00008', '1236987456321', 'Jean d\'Ormesson', ''),
('00009', '3,21457E+12', 'Fred Vargas', 'Commissaire Adamsberg'),
('00010', '3,21457E+12', 'Manon Moreau', ''),
('00011', '3,21457E+12', 'Victoria Hislop', ''),
('00012', '3,21457E+12', 'Kate Atkinson', ''),
('00013', '3,21457E+12', 'Raymond Briggs', ''),
('00014', '3,21457E+12', 'RJ Ellory', ''),
('00015', '3,21457E+12', 'Floriane Turmeau', ''),
('00016', '3,21457E+12', 'Julian Press', ''),
('00017', '3,21457E+12', 'Philippe Masson', ''),
('00018', '3,21457E+12', '', 'Guide du Routard'),
('00019', '3,21457E+12', '', 'Guide Vert'),
('00020', '3,21457E+12', '', 'Guide Vert'),
('00021', '3,21457E+12', 'Claudie Gallay', ''),
('00022', '3,21457E+12', 'Claudie Gallay', ''),
('00023', '3,21457E+12', 'Ayrolles - Masbou', 'De cape et de crocs'),
('00024', '3,21457E+12', 'Ayrolles - Masbou', 'De cape et de crocs'),
('00025', '3,21457E+12', 'Ayrolles - Masbou', 'De cape et de crocs'),
('00026', '', 'Pierre Boulle', 'Julliard');

-- --------------------------------------------------------

--
-- Structure de la table `livres_dvd`
--

CREATE TABLE `livres_dvd` (
  `id` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `livres_dvd`
--

INSERT INTO `livres_dvd` (`id`) VALUES
('00001'),
('00002'),
('00003'),
('00004'),
('00005'),
('00006'),
('00007'),
('00008'),
('00009'),
('00010'),
('00011'),
('00012'),
('00013'),
('00014'),
('00015'),
('00016'),
('00017'),
('00018'),
('00019'),
('00020'),
('00021'),
('00022'),
('00023'),
('00024'),
('00025'),
('00026'),
('20001'),
('20002'),
('20003'),
('20004');

-- --------------------------------------------------------

--
-- Structure de la table `public`
--

CREATE TABLE `public` (
  `id` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `libelle` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `public`
--

INSERT INTO `public` (`id`, `libelle`) VALUES
('00001', 'Jeunesse'),
('00002', 'Adultes'),
('00003', 'Tous publics'),
('00004', 'Ados');

-- --------------------------------------------------------

--
-- Structure de la table `rayon`
--

CREATE TABLE `rayon` (
  `id` char(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `libelle` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `rayon`
--

INSERT INTO `rayon` (`id`, `libelle`) VALUES
('BD001', 'BD Adultes'),
('BL001', 'Beaux Livres'),
('DF001', 'DVD films'),
('DV001', 'Sciences'),
('DV002', 'Maison'),
('DV003', 'Santé'),
('DV004', 'Littérature classique'),
('DV005', 'Voyages'),
('JN001', 'Jeunesse BD'),
('JN002', 'Jeunesse romans'),
('LV001', 'Littérature étrangère'),
('LV002', 'Littérature française'),
('LV003', 'Policiers français étrangers'),
('PR001', 'Presse quotidienne'),
('PR002', 'Magazines');

-- --------------------------------------------------------

--
-- Structure de la table `revue`
--

CREATE TABLE `revue` (
  `id` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `empruntable` tinyint(1) DEFAULT NULL,
  `periodicite` varchar(2) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `delaiMiseADispo` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `revue`
--

INSERT INTO `revue` (`id`, `empruntable`, `periodicite`, `delaiMiseADispo`) VALUES
('10001', 1, 'MS', 52),
('10002', 1, 'MS', 52),
('10003', 1, 'HB', 15),
('10004', 1, 'HB', 15),
('10005', 0, 'QT', 5),
('10006', 0, 'QT', 5),
('10007', 1, 'HB', 26),
('10008', 1, 'HB', 26),
('10009', 0, 'QT', 5),
('10010', 1, 'HB', 12),
('10011', 1, 'MS', 52);

-- --------------------------------------------------------

--
-- Structure de la table `service`
--

CREATE TABLE `service` (
  `id` int(11) NOT NULL,
  `service` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `service`
--

INSERT INTO `service` (`id`, `service`) VALUES
(1, 'administra'),
(2, 'administra'),
(3, 'prets'),
(4, 'culture');

-- --------------------------------------------------------

--
-- Structure de la table `suivi`
--

CREATE TABLE `suivi` (
  `id` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `libelle` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `suivi`
--

INSERT INTO `suivi` (`id`, `libelle`) VALUES
('EC', 'En cours'),
('LI', 'Livrée'),
('REG', 'Réglée'),
('REL', 'Relancée');

-- --------------------------------------------------------

--
-- Structure de la table `test`
--

CREATE TABLE `test` (
  `password` varchar(20) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `test`
--

INSERT INTO `test` (`password`) VALUES
('password');

-- --------------------------------------------------------

--
-- Structure de la table `utilisateur`
--

CREATE TABLE `utilisateur` (
  `id` int(11) NOT NULL,
  `login` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `password` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `idservice` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `utilisateur`
--

INSERT INTO `utilisateur` (`id`, `login`, `password`, `idservice`) VALUES
(1, 'culture', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 4),
(2, 'admin', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 1),
(3, 'administratif', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 2),
(4, 'pret', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 3);

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `abonnement`
--
ALTER TABLE `abonnement`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idRevue` (`idRevue`);

--
-- Index pour la table `commande`
--
ALTER TABLE `commande`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `commandedocument`
--
ALTER TABLE `commandedocument`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idLivreDvd` (`idLivreDvd`),
  ADD KEY `commandedocument_ibfk_3` (`idsuivi`);

--
-- Index pour la table `document`
--
ALTER TABLE `document`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idRayon` (`idRayon`),
  ADD KEY `idPublic` (`idPublic`),
  ADD KEY `idGenre` (`idGenre`);

--
-- Index pour la table `dvd`
--
ALTER TABLE `dvd`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `etat`
--
ALTER TABLE `etat`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `exemplaire`
--
ALTER TABLE `exemplaire`
  ADD PRIMARY KEY (`id`,`numero`),
  ADD KEY `idEtat` (`idEtat`);

--
-- Index pour la table `genre`
--
ALTER TABLE `genre`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `livre`
--
ALTER TABLE `livre`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `livres_dvd`
--
ALTER TABLE `livres_dvd`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `public`
--
ALTER TABLE `public`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `rayon`
--
ALTER TABLE `rayon`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `revue`
--
ALTER TABLE `revue`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `service`
--
ALTER TABLE `service`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `suivi`
--
ALTER TABLE `suivi`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `utilisateur`
--
ALTER TABLE `utilisateur`
  ADD PRIMARY KEY (`id`),
  ADD KEY `commandedocument_ibfk_7` (`idservice`);

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `abonnement`
--
ALTER TABLE `abonnement`
  ADD CONSTRAINT `abonnement_ibfk_1` FOREIGN KEY (`id`) REFERENCES `commande` (`id`),
  ADD CONSTRAINT `abonnement_ibfk_2` FOREIGN KEY (`idRevue`) REFERENCES `revue` (`id`);

--
-- Contraintes pour la table `commandedocument`
--
ALTER TABLE `commandedocument`
  ADD CONSTRAINT `commandedocument_ibfk_1` FOREIGN KEY (`id`) REFERENCES `commande` (`id`),
  ADD CONSTRAINT `commandedocument_ibfk_2` FOREIGN KEY (`idLivreDvd`) REFERENCES `livres_dvd` (`id`),
  ADD CONSTRAINT `commandedocument_ibfk_3` FOREIGN KEY (`idsuivi`) REFERENCES `suivi` (`id`);

--
-- Contraintes pour la table `document`
--
ALTER TABLE `document`
  ADD CONSTRAINT `document_ibfk_1` FOREIGN KEY (`idRayon`) REFERENCES `rayon` (`id`),
  ADD CONSTRAINT `document_ibfk_2` FOREIGN KEY (`idPublic`) REFERENCES `public` (`id`),
  ADD CONSTRAINT `document_ibfk_3` FOREIGN KEY (`idGenre`) REFERENCES `genre` (`id`);

--
-- Contraintes pour la table `dvd`
--
ALTER TABLE `dvd`
  ADD CONSTRAINT `dvd_ibfk_1` FOREIGN KEY (`id`) REFERENCES `livres_dvd` (`id`);

--
-- Contraintes pour la table `exemplaire`
--
ALTER TABLE `exemplaire`
  ADD CONSTRAINT `exemplaire_ibfk_1` FOREIGN KEY (`id`) REFERENCES `document` (`id`),
  ADD CONSTRAINT `exemplaire_ibfk_2` FOREIGN KEY (`idEtat`) REFERENCES `etat` (`id`);

--
-- Contraintes pour la table `livre`
--
ALTER TABLE `livre`
  ADD CONSTRAINT `livre_ibfk_1` FOREIGN KEY (`id`) REFERENCES `livres_dvd` (`id`);

--
-- Contraintes pour la table `livres_dvd`
--
ALTER TABLE `livres_dvd`
  ADD CONSTRAINT `livres_dvd_ibfk_1` FOREIGN KEY (`id`) REFERENCES `document` (`id`);

--
-- Contraintes pour la table `revue`
--
ALTER TABLE `revue`
  ADD CONSTRAINT `revue_ibfk_1` FOREIGN KEY (`id`) REFERENCES `document` (`id`);

--
-- Contraintes pour la table `utilisateur`
--
ALTER TABLE `utilisateur`
  ADD CONSTRAINT `commandedocument_ibfk_7` FOREIGN KEY (`idservice`) REFERENCES `service` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
