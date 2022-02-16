-- --------------------------------------------------------
-- Servidor:                     127.0.0.1
-- Versão do servidor:           8.0.28 - MySQL Community Server - GPL
-- OS do Servidor:               Win64
-- HeidiSQL Versão:              11.2.0.6213
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Copiando estrutura do banco de dados para gdmo
CREATE DATABASE IF NOT EXISTS `gdmo` /*!40100 DEFAULT CHARACTER SET latin1 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `gdmo`;

-- Copiando estrutura para tabela gdmo.acct
CREATE TABLE IF NOT EXISTS `acct` (
  `accountId` int NOT NULL AUTO_INCREMENT,
  `discordid` bigint DEFAULT NULL,
  `username` varchar(30) NOT NULL,
  `password` varchar(128) NOT NULL,
  `securitycode` varchar(32) DEFAULT 'c4ca4238a0b923820dcc509a6f75849b',
  `membership` int NOT NULL DEFAULT '0',
  `level` int NOT NULL DEFAULT '0' COMMENT '-1 = Banned\n0 = Normal User\n1 = High\n2 = Higher\n3 = Highest\n99 = Admin',
  `email` varchar(120) DEFAULT 'a@a.a',
  `uniId` int DEFAULT NULL,
  `char1` int DEFAULT NULL,
  `char2` int DEFAULT NULL,
  `char3` int DEFAULT NULL,
  `char4` int DEFAULT NULL,
  `char5` int DEFAULT NULL,
  `lastChar` int DEFAULT NULL,
  `crowns` int DEFAULT '0',
  `cashvault` blob,
  `accountStorage` blob,
  PRIMARY KEY (`accountId`) USING BTREE,
  UNIQUE KEY `accountId_UNIQUE` (`accountId`) USING BTREE,
  UNIQUE KEY `username_UNIQUE` (`username`) USING BTREE,
  KEY `fkey_char1` (`char1`) USING BTREE,
  KEY `fkey_char5` (`char5`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC COMMENT='Accounts table';

-- Copiando dados para a tabela gdmo.acct: ~3 rows (aproximadamente)
/*!40000 ALTER TABLE `acct` DISABLE KEYS */;
REPLACE INTO `acct` (`accountId`, `discordid`, `username`, `password`, `securitycode`, `membership`, `level`, `email`, `uniId`, `char1`, `char2`, `char3`, `char4`, `char5`, `lastChar`, `crowns`, `cashvault`, `accountStorage`) VALUES
	(4, NULL, 'admin2', '8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', 'c4ca4238a0b923820dcc509a6f75849b', 0, 0, 'a@a.a', 897124018, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL);
/*!40000 ALTER TABLE `acct` ENABLE KEYS */;

-- Copiando estrutura para tabela gdmo.chars
CREATE TABLE IF NOT EXISTS `chars` (
  `characterId` int NOT NULL AUTO_INCREMENT,
  `characterPos` int DEFAULT NULL,
  `nodeleteuntil` varchar(19) DEFAULT '2000-01-01 01:01:01',
  `accountId` int NOT NULL,
  `charModel` int NOT NULL DEFAULT '80005',
  `charName` varchar(45) NOT NULL,
  `charLv` int NOT NULL DEFAULT '1',
  `partner` int DEFAULT NULL,
  `starter` int NOT NULL DEFAULT '31001',
  `mercenary1` int DEFAULT NULL,
  `mercenary2` int DEFAULT NULL,
  `mercenary3` int DEFAULT NULL,
  `mercenary4` int DEFAULT NULL,
  `mercenaryLimit` int NOT NULL DEFAULT '3',
  `money` int NOT NULL DEFAULT '0',
  `chipset` blob,
  `inventoryLimit` int DEFAULT '30',
  `inventory` blob,
  `storageLimit` int DEFAULT '21',
  `storage` blob,
  `archiveLimit` int DEFAULT '1',
  `archive` blob COMMENT 'byte[] array containing digi IDs from digimon table',
  `equipment` blob,
  `XAI` int DEFAULT '0',
  `XGauge` int NOT NULL DEFAULT '0',
  `JogChipSet` blob,
  `tempcashvault` blob,
  `experience` int NOT NULL DEFAULT '0',
  `map` int NOT NULL DEFAULT '3',
  `x` int NOT NULL DEFAULT '19996',
  `y` int NOT NULL DEFAULT '17590',
  `maxDS` int NOT NULL DEFAULT '80',
  `maxHP` int NOT NULL DEFAULT '90',
  `DS` int NOT NULL DEFAULT '80',
  `HP` int NOT NULL DEFAULT '90',
  `AT` int NOT NULL DEFAULT '10',
  `DE` int NOT NULL DEFAULT '2',
  `MS` int NOT NULL DEFAULT '550',
  `Fatigue` int NOT NULL DEFAULT '0',
  `quests` blob,
  `sealmaster` blob,
  `currentsealleader` int DEFAULT '0',
  `friendList` blob,
  `incubator` int DEFAULT '0',
  `incubatorLevel` int DEFAULT '0',
  `incubatorBackup` int DEFAULT '0',
  `title` int DEFAULT '0',
  PRIMARY KEY (`characterId`) USING BTREE,
  UNIQUE KEY `characterId_UNIQUE` (`characterId`) USING BTREE,
  UNIQUE KEY `charName_UNIQUE` (`charName`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC COMMENT='The table that contains all characters.';

-- Copiando dados para a tabela gdmo.chars: ~2 rows (aproximadamente)
/*!40000 ALTER TABLE `chars` DISABLE KEYS */;
/*!40000 ALTER TABLE `chars` ENABLE KEYS */;

-- Copiando estrutura para tabela gdmo.digimon
CREATE TABLE IF NOT EXISTS `digimon` (
  `digimonId` int NOT NULL AUTO_INCREMENT,
  `characterId` int NOT NULL,
  `digiName` varchar(45) NOT NULL DEFAULT 'Genericmon',
  `digiLv` int NOT NULL DEFAULT '1',
  `digiType` int NOT NULL DEFAULT '31003' COMMENT 'Digimon Model?',
  `digiModel` int NOT NULL DEFAULT '31003',
  `exp` int NOT NULL DEFAULT '0',
  `skillPoints` int NOT NULL DEFAULT '0',
  `skillLevel` int NOT NULL DEFAULT '1',
  `skills` blob,
  `skillGrade` blob,
  `digiSize` int NOT NULL DEFAULT '15000',
  `digiScale` int NOT NULL DEFAULT '0',
  `maxHP` int NOT NULL DEFAULT '100',
  `HP` int NOT NULL DEFAULT '100',
  `maxDS` int NOT NULL DEFAULT '100',
  `DS` int NOT NULL DEFAULT '100',
  `DE` int NOT NULL DEFAULT '100',
  `AT` int NOT NULL DEFAULT '60',
  `BL` int NOT NULL DEFAULT '0',
  `sync` int NOT NULL DEFAULT '0',
  `HT` int NOT NULL DEFAULT '0',
  `EV` int NOT NULL DEFAULT '0',
  `CR` int NOT NULL DEFAULT '0',
  `MS` int NOT NULL DEFAULT '580',
  `AS` int NOT NULL DEFAULT '3000',
  `forms` blob,
  `unlocked_levels` int DEFAULT '0',
  PRIMARY KEY (`digimonId`) USING BTREE,
  UNIQUE KEY `digimonId_UNIQUE` (`digimonId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela gdmo.digimon: ~5 rows (aproximadamente)
/*!40000 ALTER TABLE `digimon` DISABLE KEYS */;
/*!40000 ALTER TABLE `digimon` ENABLE KEYS */;

-- Copiando estrutura para tabela gdmo.guilds
CREATE TABLE IF NOT EXISTS `guilds` (
  `guildid` int NOT NULL,
  `guildname` varchar(45) DEFAULT NULL,
  `members` blob
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC COMMENT='A table of servers and ips';

-- Copiando dados para a tabela gdmo.guilds: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `guilds` DISABLE KEYS */;
/*!40000 ALTER TABLE `guilds` ENABLE KEYS */;

-- Copiando estrutura para tabela gdmo.servers
CREATE TABLE IF NOT EXISTS `servers` (
  `serverId` int NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  `ip` mediumtext,
  `port` int DEFAULT NULL,
  `maintenance` int DEFAULT '0',
  `serverload` int DEFAULT '0',
  `isNew?` int DEFAULT '0',
  PRIMARY KEY (`serverId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC COMMENT='A table of servers and ips';

-- Copiando dados para a tabela gdmo.servers: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `servers` DISABLE KEYS */;
REPLACE INTO `servers` (`serverId`, `name`, `ip`, `port`, `maintenance`, `serverload`, `isNew?`) VALUES
	(1001, 'LOCAL', '2130706433', 7030, 0, 0, 0);
/*!40000 ALTER TABLE `servers` ENABLE KEYS */;

-- Copiando estrutura para tabela gdmo.settings
CREATE TABLE IF NOT EXISTS `settings` (
  `status` int NOT NULL,
  `maintenance` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela gdmo.settings: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
/*!40000 ALTER TABLE `settings` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
