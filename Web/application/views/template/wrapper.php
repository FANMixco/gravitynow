<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

/* load the header */
$this->load->view('template/header');

$this->load->view($mainView);

/* load the footer */
$this->load->view('template/footer');