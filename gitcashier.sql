-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Waktu pembuatan: 31 Jul 2024 pada 16.45
-- Versi server: 8.0.30
-- Versi PHP: 8.3.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `gitcashier`
--

-- --------------------------------------------------------

--
-- Struktur dari tabel `pembayaran`
--

CREATE TABLE `pembayaran` (
  `id` int NOT NULL,
  `tgl_pembelian` date NOT NULL,
  `tunai` decimal(10,0) NOT NULL,
  `tgl_pembayaran` date NOT NULL,
  `faktur` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `jenis` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktur dari tabel `product`
--

CREATE TABLE `product` (
  `id` int NOT NULL,
  `kode_brg` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `nama_brg` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `stok_awal` int DEFAULT NULL,
  `masuk` int DEFAULT '0',
  `keluar` int DEFAULT '0',
  `stok_akhir` int DEFAULT '0',
  `suplier` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '-',
  `beli` decimal(10,2) DEFAULT NULL,
  `jual` decimal(10,2) DEFAULT NULL,
  `mark_up` decimal(10,2) DEFAULT NULL,
  `pendapatan` decimal(10,2) DEFAULT NULL,
  `laba` decimal(10,2) DEFAULT NULL,
  `harta` decimal(10,2) DEFAULT NULL,
  `persentase` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `cl` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Trigger `product`
--
DELIMITER $$
CREATE TRIGGER `update_harta` BEFORE UPDATE ON `product` FOR EACH ROW BEGIN
    -- Perbarui nilai stok_akhir berdasarkan perbedaan stok_awal dan stok_keluar
    SET NEW.harta = OLD.beli * NEW.stok_akhir;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `update_laba` BEFORE UPDATE ON `product` FOR EACH ROW BEGIN
    -- Perbarui nilai stok_akhir berdasarkan perbedaan stok_awal dan stok_keluar
    SET NEW.laba = NEW.mark_up * NEW.keluar;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `update_pendapatan` BEFORE UPDATE ON `product` FOR EACH ROW BEGIN
    -- Perbarui nilai stok_akhir berdasarkan perbedaan stok_awal dan stok_keluar
    SET NEW.pendapatan = NEW.jual * NEW.keluar;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `update_stok_akhir` BEFORE UPDATE ON `product` FOR EACH ROW BEGIN
    -- Perbarui nilai stok_akhir berdasarkan perbedaan stok_awal dan stok_keluar
    SET NEW.stok_akhir = (OLD.stok_awal +
 NEW.masuk)- NEW.keluar;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Struktur dari tabel `staff`
--

CREATE TABLE `staff` (
  `id` int NOT NULL,
  `User` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `jabatan` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `md` int NOT NULL,
  `mp` int NOT NULL,
  `mbm` int NOT NULL,
  `mbk` int NOT NULL,
  `mk` int NOT NULL,
  `lbm` int NOT NULL,
  `lbk` int NOT NULL,
  `lh` int NOT NULL,
  `lp` int NOT NULL,
  `llss` int NOT NULL,
  `cl` int NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `staff`
--

INSERT INTO `staff` (`id`, `User`, `Password`, `jabatan`, `md`, `mp`, `mbm`, `mbk`, `mk`, `lbm`, `lbk`, `lh`, `lp`, `llss`, `cl`) VALUES
(1, 'admin', 'admin', 'admin', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1),
(2, 'manager', '12345678', 'manager', 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0),
(3, 'test', 'test', 'kasir', 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);

-- --------------------------------------------------------

--
-- Struktur dari tabel `tablename`
--

CREATE TABLE `tablename` (
  `id` int NOT NULL,
  `kode_brg` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `nama_brg` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `stok_akhir` int DEFAULT NULL,
  `beli` decimal(10,2) DEFAULT NULL,
  `jual` decimal(10,2) DEFAULT NULL,
  `mark_up` decimal(10,2) DEFAULT NULL,
  `harta` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktur dari tabel `transaction`
--

CREATE TABLE `transaction` (
  `id` int NOT NULL,
  `no_faktur` int NOT NULL,
  `kode` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `tgl` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `tgl_pelunasan` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `nama` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `qty` int NOT NULL,
  `harga` decimal(10,2) NOT NULL,
  `subtotal` decimal(10,2) NOT NULL,
  `mark_up` decimal(10,2) NOT NULL,
  `laba` decimal(10,2) NOT NULL,
  `payment` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'tunai',
  `namaPelanggan` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '-',
  `Tunai` decimal(10,0) NOT NULL,
  `status` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '-',
  `retur` int NOT NULL,
  `debug` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Trigger `transaction`
--
DELIMITER $$
CREATE TRIGGER `update subtotal` BEFORE UPDATE ON `transaction` FOR EACH ROW BEGIN
    -- Perbarui nilai stok_akhir berdasarkan perbedaan stok_awal dan stok_keluar
    SET NEW.subtotal = NEW.qty * OLD.harga;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Struktur dari tabel `transaction_in`
--

CREATE TABLE `transaction_in` (
  `id` int NOT NULL,
  `tgl` date NOT NULL,
  `no_faktur` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `kode` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `nama` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `qty` int NOT NULL,
  `suplier` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `payment` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `harga` decimal(10,0) NOT NULL,
  `subtotal` decimal(10,0) NOT NULL,
  `retur` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indeks untuk tabel `pembayaran`
--
ALTER TABLE `pembayaran`
  ADD PRIMARY KEY (`id`);

--
-- Indeks untuk tabel `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`id`);

--
-- Indeks untuk tabel `staff`
--
ALTER TABLE `staff`
  ADD PRIMARY KEY (`id`);

--
-- Indeks untuk tabel `tablename`
--
ALTER TABLE `tablename`
  ADD PRIMARY KEY (`id`);

--
-- Indeks untuk tabel `transaction`
--
ALTER TABLE `transaction`
  ADD PRIMARY KEY (`id`);

--
-- Indeks untuk tabel `transaction_in`
--
ALTER TABLE `transaction_in`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT untuk tabel yang dibuang
--

--
-- AUTO_INCREMENT untuk tabel `pembayaran`
--
ALTER TABLE `pembayaran`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT untuk tabel `product`
--
ALTER TABLE `product`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT untuk tabel `staff`
--
ALTER TABLE `staff`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT untuk tabel `transaction`
--
ALTER TABLE `transaction`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT untuk tabel `transaction_in`
--
ALTER TABLE `transaction_in`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
