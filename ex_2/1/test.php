<?php

require_once('../_helpers/strip.php');

// https://depthsecurity.com/blog/exploitation-xml-external-entity-xxe-injection

// Enable protection against XXE
libxml_disable_entity_loader(true);

$xml = strlen($_GET['xml']) > 0 ? $_GET['xml'] : '<root><content>No XML found</content></root>';

$document = new DOMDocument();
$document->loadXML($xml);
$parsedDocument = simplexml_import_dom($document);

echo $parsedDocument->content;
