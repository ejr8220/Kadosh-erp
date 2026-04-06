SET NOCOUNT ON;
SET XACT_ABORT ON;
/*
  Nota de alcance:
  - Paises: catalogo global completo (250 registros) con IDs manuales.
  - Provincias: 24 provincias de Ecuador.
  - Ciudades: catalogo publico de Ecuador (114 registros).
  - Parroquias: carga inicial espejada 1:1 desde el catalogo de ciudades para mantener FK determinista.
*/
BEGIN TRY
BEGIN TRAN;
DECLARE @Now DATETIME2 = SYSUTCDATETIME();
DECLARE @SeedUser NVARCHAR(30) = N'seed-script';
DECLARE @MaxId INT;

IF NOT EXISTS (SELECT 1 FROM Countries)
BEGIN
DBCC CHECKIDENT ('Countries', RESEED, 0);
SET IDENTITY_INSERT Countries ON;
INSERT INTO Countries (Id, Name, IsoCode, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, N'Ecuador', 'ECU', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, N'Ãland Islands', 'ALA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, N'Afghanistan', 'AFG', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(4, N'Albania', 'ALB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(5, N'Algeria', 'DZA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(6, N'American Samoa', 'ASM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(7, N'Andorra', 'AND', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(8, N'Angola', 'AGO', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(9, N'Anguilla', 'AIA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(10, N'Antarctica', 'ATA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(11, N'Antigua and Barbuda', 'ATG', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(12, N'Argentina', 'ARG', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(13, N'Armenia', 'ARM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(14, N'Aruba', 'ABW', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(15, N'Australia', 'AUS', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(16, N'Austria', 'AUT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(17, N'Azerbaijan', 'AZE', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(18, N'Bahamas', 'BHS', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(19, N'Bahrain', 'BHR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(20, N'Bangladesh', 'BGD', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(21, N'Barbados', 'BRB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(22, N'Belarus', 'BLR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(23, N'Belgium', 'BEL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(24, N'Belize', 'BLZ', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(25, N'Benin', 'BEN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(26, N'Bermuda', 'BMU', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(27, N'Bhutan', 'BTN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(28, N'Bolivia', 'BOL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(29, N'Bosnia and Herzegovina', 'BIH', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(30, N'Botswana', 'BWA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(31, N'Bouvet Island', 'BVT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(32, N'Brazil', 'BRA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(33, N'British Indian Ocean Territory', 'IOT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(34, N'British Virgin Islands', 'VGB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(35, N'Brunei', 'BRN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(36, N'Bulgaria', 'BGR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(37, N'Burkina Faso', 'BFA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(38, N'Burundi', 'BDI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(39, N'Cambodia', 'KHM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(40, N'Cameroon', 'CMR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(41, N'Canada', 'CAN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(42, N'Cape Verde', 'CPV', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(43, N'Caribbean Netherlands', 'BES', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(44, N'Cayman Islands', 'CYM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(45, N'Central African Republic', 'CAF', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(46, N'Chad', 'TCD', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(47, N'Chile', 'CHL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(48, N'China', 'CHN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(49, N'Christmas Island', 'CXR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(50, N'Cocos (Keeling) Islands', 'CCK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(51, N'Colombia', 'COL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(52, N'Comoros', 'COM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(53, N'Cook Islands', 'COK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(54, N'Costa Rica', 'CRI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(55, N'Croatia', 'HRV', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(56, N'Cuba', 'CUB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(57, N'CuraÃ§ao', 'CUW', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(58, N'Cyprus', 'CYP', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(59, N'Czechia', 'CZE', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(60, N'Denmark', 'DNK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(61, N'Djibouti', 'DJI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(62, N'Dominica', 'DMA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(63, N'Dominican Republic', 'DOM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(64, N'DR Congo', 'COD', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(65, N'Egypt', 'EGY', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(66, N'El Salvador', 'SLV', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(67, N'Equatorial Guinea', 'GNQ', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(68, N'Eritrea', 'ERI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(69, N'Estonia', 'EST', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(70, N'Eswatini', 'SWZ', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(71, N'Ethiopia', 'ETH', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(72, N'Falkland Islands', 'FLK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(73, N'Faroe Islands', 'FRO', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(74, N'Fiji', 'FJI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(75, N'Finland', 'FIN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(76, N'France', 'FRA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(77, N'French Guiana', 'GUF', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(78, N'French Polynesia', 'PYF', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(79, N'French Southern and Antarctic Lands', 'ATF', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(80, N'Gabon', 'GAB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(81, N'Gambia', 'GMB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(82, N'Georgia', 'GEO', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(83, N'Germany', 'DEU', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(84, N'Ghana', 'GHA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(85, N'Gibraltar', 'GIB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(86, N'Greece', 'GRC', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(87, N'Greenland', 'GRL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(88, N'Grenada', 'GRD', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(89, N'Guadeloupe', 'GLP', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(90, N'Guam', 'GUM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(91, N'Guatemala', 'GTM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(92, N'Guernsey', 'GGY', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(93, N'Guinea', 'GIN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(94, N'Guinea-Bissau', 'GNB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(95, N'Guyana', 'GUY', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(96, N'Haiti', 'HTI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(97, N'Heard Island and McDonald Islands', 'HMD', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(98, N'Honduras', 'HND', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(99, N'Hong Kong', 'HKG', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(100, N'Hungary', 'HUN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(101, N'Iceland', 'ISL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(102, N'India', 'IND', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(103, N'Indonesia', 'IDN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(104, N'Iran', 'IRN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(105, N'Iraq', 'IRQ', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(106, N'Ireland', 'IRL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(107, N'Isle of Man', 'IMN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(108, N'Israel', 'ISR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(109, N'Italy', 'ITA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(110, N'Ivory Coast', 'CIV', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(111, N'Jamaica', 'JAM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(112, N'Japan', 'JPN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(113, N'Jersey', 'JEY', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(114, N'Jordan', 'JOR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(115, N'Kazakhstan', 'KAZ', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(116, N'Kenya', 'KEN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(117, N'Kiribati', 'KIR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(118, N'Kosovo', 'UNK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(119, N'Kuwait', 'KWT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(120, N'Kyrgyzstan', 'KGZ', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(121, N'Laos', 'LAO', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(122, N'Latvia', 'LVA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(123, N'Lebanon', 'LBN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(124, N'Lesotho', 'LSO', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(125, N'Liberia', 'LBR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(126, N'Libya', 'LBY', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(127, N'Liechtenstein', 'LIE', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(128, N'Lithuania', 'LTU', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(129, N'Luxembourg', 'LUX', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(130, N'Macau', 'MAC', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(131, N'Madagascar', 'MDG', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(132, N'Malawi', 'MWI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(133, N'Malaysia', 'MYS', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(134, N'Maldives', 'MDV', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(135, N'Mali', 'MLI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(136, N'Malta', 'MLT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(137, N'Marshall Islands', 'MHL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(138, N'Martinique', 'MTQ', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(139, N'Mauritania', 'MRT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(140, N'Mauritius', 'MUS', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(141, N'Mayotte', 'MYT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(142, N'Mexico', 'MEX', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(143, N'Micronesia', 'FSM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(144, N'Moldova', 'MDA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(145, N'Monaco', 'MCO', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(146, N'Mongolia', 'MNG', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(147, N'Montenegro', 'MNE', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(148, N'Montserrat', 'MSR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(149, N'Morocco', 'MAR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(150, N'Mozambique', 'MOZ', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(151, N'Myanmar', 'MMR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(152, N'Namibia', 'NAM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(153, N'Nauru', 'NRU', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(154, N'Nepal', 'NPL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(155, N'Netherlands', 'NLD', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(156, N'New Caledonia', 'NCL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(157, N'New Zealand', 'NZL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(158, N'Nicaragua', 'NIC', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(159, N'Niger', 'NER', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(160, N'Nigeria', 'NGA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(161, N'Niue', 'NIU', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(162, N'Norfolk Island', 'NFK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(163, N'North Korea', 'PRK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(164, N'North Macedonia', 'MKD', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(165, N'Northern Mariana Islands', 'MNP', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(166, N'Norway', 'NOR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(167, N'Oman', 'OMN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(168, N'Pakistan', 'PAK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(169, N'Palau', 'PLW', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(170, N'Palestine', 'PSE', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(171, N'Panama', 'PAN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(172, N'Papua New Guinea', 'PNG', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(173, N'Paraguay', 'PRY', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(174, N'Peru', 'PER', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(175, N'Philippines', 'PHL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(176, N'Pitcairn Islands', 'PCN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(177, N'Poland', 'POL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(178, N'Portugal', 'PRT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(179, N'Puerto Rico', 'PRI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(180, N'Qatar', 'QAT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(181, N'RÃ©union', 'REU', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(182, N'Republic of the Congo', 'COG', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(183, N'Romania', 'ROU', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(184, N'Russia', 'RUS', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(185, N'Rwanda', 'RWA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(186, N'SÃ£o TomÃ© and PrÃ­ncipe', 'STP', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(187, N'Saint BarthÃ©lemy', 'BLM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(188, N'Saint Helena, Ascension and Tristan da Cunha', 'SHN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(189, N'Saint Kitts and Nevis', 'KNA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(190, N'Saint Lucia', 'LCA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(191, N'Saint Martin', 'MAF', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(192, N'Saint Pierre and Miquelon', 'SPM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(193, N'Saint Vincent and the Grenadines', 'VCT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(194, N'Samoa', 'WSM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(195, N'San Marino', 'SMR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(196, N'Saudi Arabia', 'SAU', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(197, N'Senegal', 'SEN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(198, N'Serbia', 'SRB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(199, N'Seychelles', 'SYC', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(200, N'Sierra Leone', 'SLE', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(201, N'Singapore', 'SGP', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(202, N'Sint Maarten', 'SXM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(203, N'Slovakia', 'SVK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(204, N'Slovenia', 'SVN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(205, N'Solomon Islands', 'SLB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(206, N'Somalia', 'SOM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(207, N'South Africa', 'ZAF', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(208, N'South Georgia', 'SGS', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(209, N'South Korea', 'KOR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(210, N'South Sudan', 'SSD', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(211, N'Spain', 'ESP', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(212, N'Sri Lanka', 'LKA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(213, N'Sudan', 'SDN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(214, N'Suriname', 'SUR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(215, N'Svalbard and Jan Mayen', 'SJM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(216, N'Sweden', 'SWE', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(217, N'Switzerland', 'CHE', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(218, N'Syria', 'SYR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(219, N'Taiwan', 'TWN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(220, N'Tajikistan', 'TJK', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(221, N'Tanzania', 'TZA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(222, N'Thailand', 'THA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(223, N'Timor-Leste', 'TLS', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(224, N'Togo', 'TGO', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(225, N'Tokelau', 'TKL', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(226, N'Tonga', 'TON', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(227, N'Trinidad and Tobago', 'TTO', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(228, N'Tunisia', 'TUN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(229, N'Turkey', 'TUR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(230, N'Turkmenistan', 'TKM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(231, N'Turks and Caicos Islands', 'TCA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(232, N'Tuvalu', 'TUV', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(233, N'Uganda', 'UGA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(234, N'Ukraine', 'UKR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(235, N'United Arab Emirates', 'ARE', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(236, N'United Kingdom', 'GBR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(237, N'United States', 'USA', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(238, N'United States Minor Outlying Islands', 'UMI', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(239, N'United States Virgin Islands', 'VIR', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(240, N'Uruguay', 'URY', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(241, N'Uzbekistan', 'UZB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(242, N'Vanuatu', 'VUT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(243, N'Vatican City', 'VAT', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(244, N'Venezuela', 'VEN', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(245, N'Vietnam', 'VNM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(246, N'Wallis and Futuna', 'WLF', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(247, N'Western Sahara', 'ESH', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(248, N'Yemen', 'YEM', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(249, N'Zambia', 'ZMB', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(250, N'Zimbabwe', 'ZWE', @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT Countries OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Countries; DBCC CHECKIDENT ('Countries', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Countries;
  DBCC CHECKIDENT ('Countries', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM Provinces)
BEGIN
DBCC CHECKIDENT ('Provinces', RESEED, 0);
SET IDENTITY_INSERT Provinces ON;
INSERT INTO Provinces (Id, Name, CountryId, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, N'Azuay', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, N'Bolívar', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, N'Cañar', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(4, N'Carchi', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(5, N'Chimborazo', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(6, N'Cotopaxi', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(7, N'El Oro', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(8, N'Esmeraldas', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(9, N'Galápagos', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(10, N'Guayas', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(11, N'Imbabura', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(12, N'Loja', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(13, N'Los Ríos', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(14, N'Manabí', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(15, N'Morona-Santiago', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(16, N'Napo', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(17, N'Orellana', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(18, N'Pastaza', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(19, N'Pichincha', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(20, N'Santa Elena', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(21, N'Santo Domingo de los Tsáchilas', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(22, N'Sucumbíos', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(23, N'Tungurahua', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(24, N'Zamora Chinchipe', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT Provinces OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Provinces; DBCC CHECKIDENT ('Provinces', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Provinces;
  DBCC CHECKIDENT ('Provinces', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM Cities)
BEGIN
DBCC CHECKIDENT ('Cities', RESEED, 0);
SET IDENTITY_INSERT Cities ON;
INSERT INTO Cities (Id, Name, ProvinceId, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, N'Alausí', 5, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, N'Alfredo Baquerizo Moreno', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, N'Ambato', 23, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(4, N'Archidona', 16, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(5, N'Atuntaqui', 11, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(6, N'Azogues', 3, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(7, N'Babahoyo', 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(8, N'Bahía de Caráquez', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(9, N'Baláo', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(10, N'Balzar', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(11, N'Baños', 23, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(12, N'Boca Suno', 17, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(13, N'Calceta', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(14, N'Cañar', 3, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(15, N'Cantón Portoviejo', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(16, N'Cantón San Fernando', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(17, N'Catarama', 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(18, N'Cayambe', 19, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(19, N'Chone', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(20, N'Colimes', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(21, N'Coronel Marcelino Maridueña', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(22, N'Cotacachi', 11, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(23, N'Cuenca', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(24, N'El Ángel', 4, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(25, N'El Triunfo', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(26, N'Eloy Alfaro', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(27, N'Esmeraldas', 8, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(28, N'Francisco de Orellana Canton', 17, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(29, N'Gualaceo', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(30, N'Gualaquiza', 15, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(31, N'Guano', 5, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(32, N'Guaranda', 2, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(33, N'Guayaquil', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(34, N'Huaquillas', 7, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(35, N'Ibarra', 11, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(36, N'Jipijapa', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(37, N'Junín', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(38, N'La Libertad', 20, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(39, N'La Maná', 6, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(40, N'La Troncal', 3, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(41, N'La Unión', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(42, N'Latacunga', 6, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(43, N'Llacao', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(44, N'Loja', 12, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(45, N'Lomas de Sargentillo', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(46, N'Loreto Canton', 17, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(47, N'Macas', 15, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(48, N'Machachi', 19, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(49, N'Machala', 7, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(50, N'Manta', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(51, N'Milagro', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(52, N'Montalvo', 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(53, N'Montecristi', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(54, N'Muisne', 8, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(55, N'Naranjal', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(56, N'Naranjito', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(57, N'Nueva Loja', 22, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(58, N'Nulti', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(59, N'Otavalo', 11, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(60, N'Paján', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(61, N'Palenque', 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(62, N'Palestina', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(63, N'Palora', 15, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(64, N'Pampanal de Bolívar', 8, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(65, N'Pasaje', 7, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(66, N'Pedernales', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(67, N'Pedro Carbo', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(68, N'Pelileo', 23, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(69, N'Píllaro', 23, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(70, N'Pimampiro', 11, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(71, N'Piñas', 7, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(72, N'Playas', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(73, N'Portovelo', 7, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(74, N'Portoviejo', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(75, N'Puerto Ayora', 9, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(76, N'Puerto Baquerizo Moreno', 9, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(77, N'Puerto Bolívar', 7, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(78, N'Puerto Francisco de Orellana', 17, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(79, N'Puerto Villamil', 9, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(80, N'Pujilí', 6, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(81, N'Puyo', 18, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(82, N'Quevedo', 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(83, N'Quito', 19, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(84, N'Rio Verde', 8, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(85, N'Riobamba', 5, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(86, N'Rocafuerte', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(87, N'Rosa Zarate', 8, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(88, N'Salinas', 20, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(89, N'Samborondón', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(90, N'San Gabriel', 4, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(91, N'San Lorenzo de Esmeraldas', 8, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(92, N'San Miguel', 2, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(93, N'San Miguel de Salcedo', 6, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(94, N'San Vicente', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(95, N'Sangolquí', 19, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(96, N'Santa Ana', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(97, N'Santa Elena', 20, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(98, N'Santa Lucía', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(99, N'Santa Rosa', 7, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(100, N'Santo Domingo de los Colorados', 21, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(101, N'Saquisilí', 6, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(102, N'Sucre', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(103, N'Sucúa', 15, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(104, N'Tena', 16, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(105, N'Tosagua', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(106, N'Tulcán', 4, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(107, N'Valdez', 8, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(108, N'Velasco Ibarra', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(109, N'Ventanas', 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(110, N'Vinces', 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(111, N'Yaguachi Nuevo', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(112, N'Yantzaza', 24, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(113, N'Zamora', 24, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(114, N'Zaruma', 7, @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT Cities OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Cities; DBCC CHECKIDENT ('Cities', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Cities;
  DBCC CHECKIDENT ('Cities', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM Parishes)
BEGIN
DBCC CHECKIDENT ('Parishes', RESEED, 0);
SET IDENTITY_INSERT Parishes ON;
INSERT INTO Parishes (Id, Name, CityId, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, N'Alausí', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, N'Alfredo Baquerizo Moreno', 2, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, N'Ambato', 3, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(4, N'Archidona', 4, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(5, N'Atuntaqui', 5, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(6, N'Azogues', 6, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(7, N'Babahoyo', 7, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(8, N'Bahía de Caráquez', 8, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(9, N'Baláo', 9, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(10, N'Balzar', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(11, N'Baños', 11, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(12, N'Boca Suno', 12, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(13, N'Calceta', 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(14, N'Cañar', 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(15, N'Cantón Portoviejo', 15, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(16, N'Cantón San Fernando', 16, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(17, N'Catarama', 17, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(18, N'Cayambe', 18, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(19, N'Chone', 19, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(20, N'Colimes', 20, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(21, N'Coronel Marcelino Maridueña', 21, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(22, N'Cotacachi', 22, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(23, N'Cuenca', 23, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(24, N'El Ángel', 24, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(25, N'El Triunfo', 25, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(26, N'Eloy Alfaro', 26, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(27, N'Esmeraldas', 27, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(28, N'Francisco de Orellana Canton', 28, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(29, N'Gualaceo', 29, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(30, N'Gualaquiza', 30, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(31, N'Guano', 31, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(32, N'Guaranda', 32, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(33, N'Guayaquil', 33, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(34, N'Huaquillas', 34, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(35, N'Ibarra', 35, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(36, N'Jipijapa', 36, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(37, N'Junín', 37, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(38, N'La Libertad', 38, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(39, N'La Maná', 39, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(40, N'La Troncal', 40, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(41, N'La Unión', 41, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(42, N'Latacunga', 42, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(43, N'Llacao', 43, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(44, N'Loja', 44, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(45, N'Lomas de Sargentillo', 45, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(46, N'Loreto Canton', 46, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(47, N'Macas', 47, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(48, N'Machachi', 48, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(49, N'Machala', 49, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(50, N'Manta', 50, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(51, N'Milagro', 51, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(52, N'Montalvo', 52, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(53, N'Montecristi', 53, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(54, N'Muisne', 54, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(55, N'Naranjal', 55, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(56, N'Naranjito', 56, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(57, N'Nueva Loja', 57, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(58, N'Nulti', 58, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(59, N'Otavalo', 59, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(60, N'Paján', 60, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(61, N'Palenque', 61, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(62, N'Palestina', 62, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(63, N'Palora', 63, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(64, N'Pampanal de Bolívar', 64, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(65, N'Pasaje', 65, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(66, N'Pedernales', 66, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(67, N'Pedro Carbo', 67, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(68, N'Pelileo', 68, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(69, N'Píllaro', 69, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(70, N'Pimampiro', 70, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(71, N'Piñas', 71, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(72, N'Playas', 72, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(73, N'Portovelo', 73, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(74, N'Portoviejo', 74, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(75, N'Puerto Ayora', 75, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(76, N'Puerto Baquerizo Moreno', 76, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(77, N'Puerto Bolívar', 77, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(78, N'Puerto Francisco de Orellana', 78, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(79, N'Puerto Villamil', 79, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(80, N'Pujilí', 80, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(81, N'Puyo', 81, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(82, N'Quevedo', 82, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(83, N'Quito', 83, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(84, N'Rio Verde', 84, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(85, N'Riobamba', 85, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(86, N'Rocafuerte', 86, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(87, N'Rosa Zarate', 87, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(88, N'Salinas', 88, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(89, N'Samborondón', 89, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(90, N'San Gabriel', 90, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(91, N'San Lorenzo de Esmeraldas', 91, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(92, N'San Miguel', 92, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(93, N'San Miguel de Salcedo', 93, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(94, N'San Vicente', 94, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(95, N'Sangolquí', 95, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(96, N'Santa Ana', 96, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(97, N'Santa Elena', 97, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(98, N'Santa Lucía', 98, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(99, N'Santa Rosa', 99, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(100, N'Santo Domingo de los Colorados', 100, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(101, N'Saquisilí', 101, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(102, N'Sucre', 102, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(103, N'Sucúa', 103, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(104, N'Tena', 104, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(105, N'Tosagua', 105, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(106, N'Tulcán', 106, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(107, N'Valdez', 107, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(108, N'Velasco Ibarra', 108, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(109, N'Ventanas', 109, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(110, N'Vinces', 110, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(111, N'Yaguachi Nuevo', 111, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(112, N'Yantzaza', 112, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(113, N'Zamora', 113, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(114, N'Zaruma', 114, @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT Parishes OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Parishes; DBCC CHECKIDENT ('Parishes', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Parishes;
  DBCC CHECKIDENT ('Parishes', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM MaritalStatuses)
BEGIN
DBCC CHECKIDENT ('MaritalStatuses', RESEED, 0);
SET IDENTITY_INSERT MaritalStatuses ON;
INSERT INTO MaritalStatuses (Id, Name, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, N'Soltero', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, N'Casado', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, N'Viudo', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(4, N'Divorciado', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(5, N'Unión de hecho', @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT MaritalStatuses OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM MaritalStatuses; DBCC CHECKIDENT ('MaritalStatuses', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM MaritalStatuses;
  DBCC CHECKIDENT ('MaritalStatuses', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM IdentificationTypes)
BEGIN
DBCC CHECKIDENT ('IdentificationTypes', RESEED, 0);
SET IDENTITY_INSERT IdentificationTypes ON;
INSERT INTO IdentificationTypes (Id, Name, Code, Maxlength, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, N'Cedula', 'CED', 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, N'Ruc', 'RUC', 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, N'Pasaporte', 'PAS', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT IdentificationTypes OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM IdentificationTypes; DBCC CHECKIDENT ('IdentificationTypes', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM IdentificationTypes;
  DBCC CHECKIDENT ('IdentificationTypes', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM Roles)
BEGIN
DBCC CHECKIDENT ('Roles', RESEED, 0);
SET IDENTITY_INSERT Roles ON;
INSERT INTO Roles (Id, Name, Description, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status) VALUES (1, N'Administrador', N'Rol con acceso total al sistema', @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT Roles OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Roles; DBCC CHECKIDENT ('Roles', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Roles;
  DBCC CHECKIDENT ('Roles', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM Users)
BEGIN
DBCC CHECKIDENT ('Users', RESEED, 0);
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (Id, UserCode, PasswordHash, Email, LastPasswordChangeAt, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status) VALUES (1, 'Administrador', '100000.Mvuir2/Grp6irymqoCJ+kw==.BL/odGGRAVoX0jy5dX/9IpMWiH3ReKzRvuyQ2t9EeTk=', 'admin@kadosh.local', @Now, @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT Users OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Users; DBCC CHECKIDENT ('Users', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Users;
  DBCC CHECKIDENT ('Users', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM Persons)
BEGIN
DBCC CHECKIDENT ('Persons', RESEED, 0);
SET IDENTITY_INSERT Persons ON;
INSERT INTO dbo.Persons ([Id], IdentificationTypeId, IdentificationNumber, FirstName, LastName, BirthDate, Gender, CountryId, ProvinceId, CityId, ParishId, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, 2, '1790010017001', N'Kadosh', N'Matriz', '2000-01-01', 'N/A', 1, 10, 33, 33, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, 2, '1790010018001', N'Kadosh', N'Norte', '2000-01-01', 'N/A', 1, 19, 95, 95, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, 2, '1790010019001', N'Kadosh', N'Sur', '2000-01-01', 'N/A', 1, 1, 23, 23, @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT Persons OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Persons; DBCC CHECKIDENT ('Persons', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Persons;
  DBCC CHECKIDENT ('Persons', RESEED, @MaxId);
END;

IF OBJECT_ID('dbo.CompanyTypes', 'U') IS NOT NULL
BEGIN
  IF NOT EXISTS (SELECT 1 FROM CompanyTypes)
  BEGIN
  DBCC CHECKIDENT ('CompanyTypes', RESEED, 0);
  SET IDENTITY_INSERT CompanyTypes ON;
  INSERT INTO CompanyTypes (Id, Code, Name, PersonType, Description, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
  VALUES
  (1, '1', N'Contribuyente Especial (No Exportador Habitual)', 'J', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (2, '2', N'Sociedad - Calificado como Agente Retencion', 'J', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (3, '3', N'Sociedad - NO Calificado como Agente Retención', 'J', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (4, '4', N'Persona Natural con Contabilidad - Calificado como Agente Retencion', 'N', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (5, '5', N'Persona Natural con Contabilidad - NO Calificado como Agente Retención', 'N', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (6, '6', N'Microempresa - Calificado como Agente Retencion', 'J', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (7, '7', N'Microempresa - NO Calificado como Agente Retención', 'J', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (8, '8', N'Exportador Habitual - Calificado como Agente Retencion', 'J', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (9, '9', N'Exportador Habitual - Contribuyente Especial', 'J', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (10, '10', N'Persona Natural No Obligada a Llevar Contabilidad', 'N', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (11, '10', N'Personas Naturales No obligadas a llevar Contabilidad - NO Calificado como Agente Retención', 'N', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (12, '12', N'RIMPE - Negocio Popular', 'J', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (13, '13', N'RIMPE - Emprendedor', 'J', NULL, @Now, @SeedUser, NULL, NULL, 0, 'Active');
  SET IDENTITY_INSERT CompanyTypes OFF;
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM CompanyTypes; DBCC CHECKIDENT ('CompanyTypes', RESEED, @MaxId);
  END
  ELSE
  BEGIN
    SELECT @MaxId = ISNULL(MAX(Id), 0) FROM CompanyTypes;
    DBCC CHECKIDENT ('CompanyTypes', RESEED, @MaxId);
  END;
END;

IF NOT EXISTS (SELECT 1 FROM Companies)
BEGIN
DBCC CHECKIDENT ('Companies', RESEED, 0);
SET IDENTITY_INSERT Companies ON;
INSERT INTO Companies (Id, PersonId, CompanyTypeId, LegalpresentativeIdentification, LegalRepresentative, AccountantIdentification, Accountant, LogoUrl, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, 1, 1, '1790010017001', N'Kadosh Matriz', '1712345678001', N'Contabilidad Matriz', 'https://kadosh.local/logo-matriz.png', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, 2, 2, '1790010018001', N'Kadosh Norte', '1712345678002', N'Contabilidad Norte', 'https://kadosh.local/logo-norte.png', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, 3, 2, '1790010019001', N'Kadosh Sur', '1712345678003', N'Contabilidad Sur', 'https://kadosh.local/logo-sur.png', @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT Companies OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Companies; DBCC CHECKIDENT ('Companies', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Companies;
  DBCC CHECKIDENT ('Companies', RESEED, @MaxId);
END;

IF OBJECT_ID('dbo.ContactForms', 'U') IS NOT NULL
BEGIN
  IF NOT EXISTS (SELECT 1 FROM ContactForms)
  BEGIN
  DBCC CHECKIDENT ('ContactForms', RESEED, 0);
  SET IDENTITY_INSERT ContactForms ON;
  INSERT INTO ContactForms (Id, Name, Description, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
  VALUES
  (1, N'Telefono', N'Numero de telefono principal', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (2, N'Email', N'Correo electronico', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (3, N'Instagram', N'Perfil de Instagram', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (4, N'Facebook', N'Perfil de Facebook', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (5, N'X', N'Perfil de X', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (6, N'LinkedIn', N'Perfil de LinkedIn', @Now, @SeedUser, NULL, NULL, 0, 'Active');
  SET IDENTITY_INSERT ContactForms OFF;
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM ContactForms; DBCC CHECKIDENT ('ContactForms', RESEED, @MaxId);
  END
  ELSE
  BEGIN
    SELECT @MaxId = ISNULL(MAX(Id), 0) FROM ContactForms;
    DBCC CHECKIDENT ('ContactForms', RESEED, @MaxId);
  END;
END;

IF OBJECT_ID('dbo.ContactFormPersons', 'U') IS NOT NULL
BEGIN
  IF NOT EXISTS (SELECT 1 FROM ContactFormPersons)
  BEGIN
  DBCC CHECKIDENT ('ContactFormPersons', RESEED, 0);
  SET IDENTITY_INSERT ContactFormPersons ON;
  INSERT INTO ContactFormPersons (Id, PersonId, ContactFormId, ContextType, Value, IsPrimary, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
  VALUES
  (1, 1, 1, 5, N'+593999000001', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (2, 1, 2, 5, N'contacto.matriz@kadosh.local', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (3, 1, 3, 5, N'kadosh_matriz', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (4, 1, 4, 5, N'kadosh.matriz', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (5, 1, 5, 5, N'kadosh_matriz', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (6, 1, 6, 5, N'company/kadosh-matriz', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (7, 2, 1, 5, N'+593999000002', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (8, 2, 2, 5, N'contacto.norte@kadosh.local', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (9, 2, 3, 5, N'kadosh_norte', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (10, 2, 4, 5, N'kadosh.norte', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (11, 2, 5, 5, N'kadosh_norte', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (12, 2, 6, 5, N'company/kadosh-norte', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (13, 3, 1, 5, N'+593999000003', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (14, 3, 2, 5, N'contacto.sur@kadosh.local', 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (15, 3, 3, 5, N'kadosh_sur', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (16, 3, 4, 5, N'kadosh.sur', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (17, 3, 5, 5, N'kadosh_sur', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
  (18, 3, 6, 5, N'company/kadosh-sur', 0, @Now, @SeedUser, NULL, NULL, 0, 'Active');
  SET IDENTITY_INSERT ContactFormPersons OFF;
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM ContactFormPersons; DBCC CHECKIDENT ('ContactFormPersons', RESEED, @MaxId);
  END
  ELSE
  BEGIN
    SELECT @MaxId = ISNULL(MAX(Id), 0) FROM ContactFormPersons;
    DBCC CHECKIDENT ('ContactFormPersons', RESEED, @MaxId);
  END;
END;

IF NOT EXISTS (SELECT 1 FROM CompanyUsers)
BEGIN
INSERT INTO CompanyUsers (Id, CompanyId, UserId, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, 1, 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, 2, 1, @Now, @SeedUser, NULL, NULL, 0, 'Active');
END;

IF NOT EXISTS (SELECT 1 FROM Permissions)
BEGIN
DBCC CHECKIDENT ('Permissions', RESEED, 0);
SET IDENTITY_INSERT Permissions ON;
INSERT INTO Permissions (Id, Name, Description, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, 'Branch.Create', N'Permite Create en Branch', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, 'Branch.Read', N'Permite Read en Branch', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, 'Branch.Update', N'Permite Update en Branch', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(4, 'Branch.Delete', N'Permite Delete en Branch', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(5, 'City.Create', N'Permite Create en City', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(6, 'City.Read', N'Permite Read en City', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(7, 'City.Update', N'Permite Update en City', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(8, 'City.Delete', N'Permite Delete en City', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(9, 'Company.Create', N'Permite Create en Company', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(10, 'Company.Read', N'Permite Read en Company', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(11, 'Company.Update', N'Permite Update en Company', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(12, 'Company.Delete', N'Permite Delete en Company', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(13, 'CompanyUser.Create', N'Permite Create en CompanyUser', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(14, 'CompanyUser.Read', N'Permite Read en CompanyUser', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(15, 'CompanyUser.Update', N'Permite Update en CompanyUser', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(16, 'CompanyUser.Delete', N'Permite Delete en CompanyUser', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(17, 'Country.Create', N'Permite Create en Country', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(18, 'Country.Read', N'Permite Read en Country', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(19, 'Country.Update', N'Permite Update en Country', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(20, 'Country.Delete', N'Permite Delete en Country', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(21, 'IdentificationType.Create', N'Permite Create en IdentificationType', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(22, 'IdentificationType.Read', N'Permite Read en IdentificationType', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(23, 'IdentificationType.Update', N'Permite Update en IdentificationType', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(24, 'IdentificationType.Delete', N'Permite Delete en IdentificationType', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(25, 'MaritalStatus.Create', N'Permite Create en MaritalStatus', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(26, 'MaritalStatus.Read', N'Permite Read en MaritalStatus', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(27, 'MaritalStatus.Update', N'Permite Update en MaritalStatus', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(28, 'MaritalStatus.Delete', N'Permite Delete en MaritalStatus', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(29, 'Parameter.Create', N'Permite Create en Parameter', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(30, 'Parameter.Read', N'Permite Read en Parameter', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(31, 'Parameter.Update', N'Permite Update en Parameter', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(32, 'Parameter.Delete', N'Permite Delete en Parameter', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(33, 'Parish.Create', N'Permite Create en Parish', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(34, 'Parish.Read', N'Permite Read en Parish', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(35, 'Parish.Update', N'Permite Update en Parish', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(36, 'Parish.Delete', N'Permite Delete en Parish', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(37, 'Permission.Create', N'Permite Create en Permission', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(38, 'Permission.Read', N'Permite Read en Permission', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(39, 'Permission.Update', N'Permite Update en Permission', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(40, 'Permission.Delete', N'Permite Delete en Permission', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(41, 'Person.Create', N'Permite Create en Person', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(42, 'Person.Read', N'Permite Read en Person', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(43, 'Person.Update', N'Permite Update en Person', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(44, 'Person.Delete', N'Permite Delete en Person', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(45, 'Province.Create', N'Permite Create en Province', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(46, 'Province.Read', N'Permite Read en Province', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(47, 'Province.Update', N'Permite Update en Province', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(48, 'Province.Delete', N'Permite Delete en Province', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(49, 'Role.Create', N'Permite Create en Role', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(50, 'Role.Read', N'Permite Read en Role', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(51, 'Role.Update', N'Permite Update en Role', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(52, 'Role.Delete', N'Permite Delete en Role', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(53, 'RolePermission.Create', N'Permite Create en RolePermission', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(54, 'RolePermission.Read', N'Permite Read en RolePermission', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(55, 'RolePermission.Update', N'Permite Update en RolePermission', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(56, 'RolePermission.Delete', N'Permite Delete en RolePermission', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(57, 'User.Create', N'Permite Create en User', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(58, 'User.Read', N'Permite Read en User', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(59, 'User.Update', N'Permite Update en User', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(60, 'User.Delete', N'Permite Delete en User', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(61, 'UserRole.Create', N'Permite Create en UserRole', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(62, 'UserRole.Read', N'Permite Read en UserRole', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(63, 'UserRole.Update', N'Permite Update en UserRole', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(64, 'UserRole.Delete', N'Permite Delete en UserRole', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(65, 'Zone.Create', N'Permite Create en Zone', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(66, 'Zone.Read', N'Permite Read en Zone', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(67, 'Zone.Update', N'Permite Update en Zone', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(68, 'Zone.Delete', N'Permite Delete en Zone', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(69, 'TaxType.Create', N'Permite Create en TaxType', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(70, 'TaxType.Read', N'Permite Read en TaxType', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(71, 'TaxType.Update', N'Permite Update en TaxType', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(72, 'TaxType.Delete', N'Permite Delete en TaxType', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(73, 'TaxTypeDetail.Create', N'Permite Create en TaxTypeDetail', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(74, 'TaxTypeDetail.Read', N'Permite Read en TaxTypeDetail', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(75, 'TaxTypeDetail.Update', N'Permite Update en TaxTypeDetail', @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(76, 'TaxTypeDetail.Delete', N'Permite Delete en TaxTypeDetail', @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT Permissions OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Permissions; DBCC CHECKIDENT ('Permissions', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM Permissions;
  DBCC CHECKIDENT ('Permissions', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM UserRoles)
BEGIN
DBCC CHECKIDENT ('UserRoles', RESEED, 0);
SET IDENTITY_INSERT UserRoles ON;
INSERT INTO UserRoles (Id, UserId, RoleId, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES (1, 1, 1, @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT UserRoles OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM UserRoles; DBCC CHECKIDENT ('UserRoles', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM UserRoles;
  DBCC CHECKIDENT ('UserRoles', RESEED, @MaxId);
END;

IF NOT EXISTS (SELECT 1 FROM RolePermissions)
BEGIN
DBCC CHECKIDENT ('RolePermissions', RESEED, 0);
SET IDENTITY_INSERT RolePermissions ON;
INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
VALUES
(1, 1, 1, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(2, 1, 2, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(3, 1, 3, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(4, 1, 4, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(5, 1, 5, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(6, 1, 6, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(7, 1, 7, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(8, 1, 8, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(9, 1, 9, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(10, 1, 10, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(11, 1, 11, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(12, 1, 12, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(13, 1, 13, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(14, 1, 14, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(15, 1, 15, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(16, 1, 16, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(17, 1, 17, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(18, 1, 18, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(19, 1, 19, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(20, 1, 20, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(21, 1, 21, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(22, 1, 22, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(23, 1, 23, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(24, 1, 24, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(25, 1, 25, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(26, 1, 26, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(27, 1, 27, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(28, 1, 28, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(29, 1, 29, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(30, 1, 30, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(31, 1, 31, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(32, 1, 32, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(33, 1, 33, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(34, 1, 34, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(35, 1, 35, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(36, 1, 36, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(37, 1, 37, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(38, 1, 38, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(39, 1, 39, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(40, 1, 40, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(41, 1, 41, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(42, 1, 42, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(43, 1, 43, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(44, 1, 44, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(45, 1, 45, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(46, 1, 46, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(47, 1, 47, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(48, 1, 48, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(49, 1, 49, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(50, 1, 50, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(51, 1, 51, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(52, 1, 52, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(53, 1, 53, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(54, 1, 54, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(55, 1, 55, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(56, 1, 56, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(57, 1, 57, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(58, 1, 58, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(59, 1, 59, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(60, 1, 60, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(61, 1, 61, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(62, 1, 62, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(63, 1, 63, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(64, 1, 64, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(65, 1, 65, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(66, 1, 66, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(67, 1, 67, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(68, 1, 68, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(69, 1, 69, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(70, 1, 70, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(71, 1, 71, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(72, 1, 72, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(73, 1, 73, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(74, 1, 74, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(75, 1, 75, @Now, @SeedUser, NULL, NULL, 0, 'Active'),
(76, 1, 76, @Now, @SeedUser, NULL, NULL, 0, 'Active');
SET IDENTITY_INSERT RolePermissions OFF;
SELECT @MaxId = ISNULL(MAX(Id), 0) FROM RolePermissions; DBCC CHECKIDENT ('RolePermissions', RESEED, @MaxId);
END
ELSE
BEGIN
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM RolePermissions;
  DBCC CHECKIDENT ('RolePermissions', RESEED, @MaxId);
END;

IF OBJECT_ID('dbo.TaxTypes', 'U') IS NOT NULL
BEGIN
  IF NOT EXISTS (SELECT 1 FROM TaxTypes)
  BEGIN
  DBCC CHECKIDENT ('TaxTypes', RESEED, 0);
  SET IDENTITY_INSERT TaxTypes ON;
  INSERT INTO TaxTypes (Id, Name, Description, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
  VALUES (1, N'IVA', N'Impuesto al valor agregado', @Now, @SeedUser, NULL, NULL, 0, 'Active');
  SET IDENTITY_INSERT TaxTypes OFF;
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM TaxTypes; DBCC CHECKIDENT ('TaxTypes', RESEED, @MaxId);
  END
  ELSE
  BEGIN
    SELECT @MaxId = ISNULL(MAX(Id), 0) FROM TaxTypes;
    DBCC CHECKIDENT ('TaxTypes', RESEED, @MaxId);
  END;
END;

IF OBJECT_ID('dbo.TaxTypeDetails', 'U') IS NOT NULL
BEGIN
  IF NOT EXISTS (SELECT 1 FROM TaxTypeDetails)
  BEGIN
  DBCC CHECKIDENT ('TaxTypeDetails', RESEED, 0);
  SET IDENTITY_INSERT TaxTypeDetails ON;
  INSERT INTO TaxTypeDetails (Id, TaxTypeId, Value, Rate, Description, StartDate, EndDate, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted, Status)
  VALUES (1, 1, 15, 0.15, N'Porcentaje de Iva', CAST(@Now AS DATE), '2100-12-31', @Now, @SeedUser, NULL, NULL, 0, 'Active');
  SET IDENTITY_INSERT TaxTypeDetails OFF;
  SELECT @MaxId = ISNULL(MAX(Id), 0) FROM TaxTypeDetails; DBCC CHECKIDENT ('TaxTypeDetails', RESEED, @MaxId);
  END
  ELSE
  BEGIN
    SELECT @MaxId = ISNULL(MAX(Id), 0) FROM TaxTypeDetails;
    DBCC CHECKIDENT ('TaxTypeDetails', RESEED, @MaxId);
  END;
END;

COMMIT;
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0 ROLLBACK;
THROW;
END CATCH;