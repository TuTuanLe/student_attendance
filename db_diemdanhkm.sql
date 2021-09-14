-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 06, 2021 at 03:57 AM
-- Server version: 10.4.11-MariaDB
-- PHP Version: 7.4.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_diemdanhkm`
--

-- --------------------------------------------------------

--
-- Table structure for table `ct_diemdanh`
--

CREATE TABLE `ct_diemdanh` (
  `id` int(11) NOT NULL,
  `madiemdanh` int(11) NOT NULL,
  `ngay` int(11) NOT NULL,
  `lido` varchar(100) NOT NULL,
  `sogiotre` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `ct_diemdanh`
--

INSERT INTO `ct_diemdanh` (`id`, `madiemdanh`, `ngay`, `lido`, `sogiotre`) VALUES
(1, 1, 3, '1/4/2021 8:12:19 AM', 2),
(2, 3, 4, '1/4/2021 8:12:19 AM', 0),
(3, 4, 4, '1/4/2021 8:12:19 AM', 0),
(5, 6, 4, '1/4/2021 8:55:16 AM', 0),
(7, 1, 4, '1/4/2021 9:02:39 AM', 0),
(8, 3, 5, '1/5/2021 8:15:17 AM', 0),
(9, 1, 6, '1/6/2021 2:54:14 AM', 0),
(10, 3, 6, '1/6/2021 2:54:31 AM', 0),
(11, 4, 6, '1/6/2021 2:56:38 AM', 0),
(12, 7, 6, '1/6/2021 3:13:10 AM', 0);

-- --------------------------------------------------------

--
-- Table structure for table `diemdanh`
--

CREATE TABLE `diemdanh` (
  `madiemdanh` int(11) NOT NULL,
  `masv` varchar(100) NOT NULL,
  `thang` int(11) NOT NULL,
  `nam` int(11) NOT NULL,
  `tinhtrang` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `diemdanh`
--

INSERT INTO `diemdanh` (`madiemdanh`, `masv`, `thang`, `nam`, `tinhtrang`) VALUES
(1, '18521598', 1, 2021, 'absent'),
(3, '11111', 1, 2021, 'absent'),
(4, '222222', 1, 2021, 'absent'),
(5, '123434', 1, 2021, 'absent'),
(6, '44444', 1, 2021, 'absent'),
(7, '12324324', 1, 2021, 'absent');

-- --------------------------------------------------------

--
-- Table structure for table `lop`
--

CREATE TABLE `lop` (
  `malop` varchar(100) NOT NULL,
  `TenLop` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `lop`
--

INSERT INTO `lop` (`malop`, `TenLop`) VALUES
('HT2018', 'HỆ THỐNG THÔNG TIN'),
('KHMT', 'KHOA HỌC MÁY TÍNH'),
('TMDT', 'THƯƠNG MẠI ĐIÊN TỬ');

-- --------------------------------------------------------

--
-- Table structure for table `sinhvien`
--

CREATE TABLE `sinhvien` (
  `masv` varchar(100) NOT NULL,
  `hoten` varchar(100) NOT NULL,
  `malop` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `sinhvien`
--

INSERT INTO `sinhvien` (`masv`, `hoten`, `malop`) VALUES
('11111', 'ronamdo', 'HT2018'),
('12324324', 'Trường Giang', 'HT2018'),
('123434', '12433', 'HT2018'),
('18521111', 'Thai Minh Thanh', 'HT2018'),
('18521319', 'Do Thanh Quyen', 'HT2018'),
('18521356', 'Pham Tien Sy', 'HT2018'),
('18521386', 'Chu Nam Thang', 'HT2018'),
('18521598', 'Le Tu Tuan', 'HT2018'),
('18524445', 'Do Khoai Mon', 'HT2018'),
('222222', 'minHo', 'HT2018'),
('44444', 'jack', 'HT2018');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `ct_diemdanh`
--
ALTER TABLE `ct_diemdanh`
  ADD PRIMARY KEY (`id`),
  ADD KEY `conty_chinhanh` (`madiemdanh`);

--
-- Indexes for table `diemdanh`
--
ALTER TABLE `diemdanh`
  ADD PRIMARY KEY (`madiemdanh`),
  ADD KEY `muonsach_docgia` (`masv`);

--
-- Indexes for table `lop`
--
ALTER TABLE `lop`
  ADD PRIMARY KEY (`malop`);

--
-- Indexes for table `sinhvien`
--
ALTER TABLE `sinhvien`
  ADD PRIMARY KEY (`masv`),
  ADD KEY `phongban_chinhanh` (`malop`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `ct_diemdanh`
--
ALTER TABLE `ct_diemdanh`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `diemdanh`
--
ALTER TABLE `diemdanh`
  MODIFY `madiemdanh` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `ct_diemdanh`
--
ALTER TABLE `ct_diemdanh`
  ADD CONSTRAINT `conty_chinhanh` FOREIGN KEY (`madiemdanh`) REFERENCES `diemdanh` (`madiemdanh`);

--
-- Constraints for table `diemdanh`
--
ALTER TABLE `diemdanh`
  ADD CONSTRAINT `muonsach_docgia` FOREIGN KEY (`masv`) REFERENCES `sinhvien` (`masv`);

--
-- Constraints for table `sinhvien`
--
ALTER TABLE `sinhvien`
  ADD CONSTRAINT `phongban_chinhanh` FOREIGN KEY (`malop`) REFERENCES `lop` (`malop`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
