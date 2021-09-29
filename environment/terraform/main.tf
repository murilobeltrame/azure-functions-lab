terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.65"
    }
  }
  required_version = ">= 0.14.9"
}

provider "azurerm" {
  features {}
}

variable "resource_group_name" { }

variable "resources_location" {
  default = "westus2"
}

variable "storage_account_name" {
  default = "saintegrationlab"
}

variable "storage_account_container_name" {
  default = "messages"
}

variable "service_bus_namespace" {
  default = "sbintegrationlab"
}

variable "cosmosdb_account_name" {
  default = "dbaintegrationlab"
}

variable "cosmosdb_database_name" {
  default = "dbintegrationlab"
}

variable "cosmosdb_database_collection_name" {
  default = "integrations"
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.resources_location
}

resource "azurerm_storage_account" "sa" {
  name = var.storage_account_name
  resource_group_name = azurerm_resource_group.rg.name
  location = var.resources_location
  account_tier = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "sa_container" {
  name = var.storage_account_container_name
  storage_account_name = azurerm_storage_account.sa.name
  container_access_type = "private"
}

resource "azurerm_servicebus_namespace" "sb" {
  name = var.service_bus_namespace
  resource_group_name = azurerm_resource_group.rg.name
  location = var.resources_location
  sku = "Standard"
}

resource "azurerm_servicebus_topic" "sb_topic_msg_dispatched" {
  name = "messagesdispatched"
  resource_group_name = azurerm_resource_group.rg.name
  namespace_name = azurerm_servicebus_namespace.sb.name
  default_message_ttl = "P14D"
  auto_delete_on_idle = "P14D"
}

resource "azurerm_servicebus_subscription" "sb_subscription_msg_dispatched" {
  name = "MessagesDispatchedSubscription"
  resource_group_name = azurerm_resource_group.rg.name
  namespace_name = azurerm_servicebus_namespace.sb.name
  topic_name = azurerm_servicebus_topic.sb_topic_msg_dispatched.name
  max_delivery_count = 10
  default_message_ttl = "P14D"
  auto_delete_on_idle = "P14D"
  dead_lettering_on_filter_evaluation_error = true
  dead_lettering_on_message_expiration = true
}

resource "azurerm_servicebus_topic" "sb_topic_msg_processed" {
  name = "messagesprocessed"
  resource_group_name = azurerm_resource_group.rg.name
  namespace_name = azurerm_servicebus_namespace.sb.name
  default_message_ttl = "P14D"
  auto_delete_on_idle = "P14D"
}

resource "azurerm_servicebus_subscription" "sb_subscription_msg_processed" {
  name = "MessagesProcessedSubscription"
  resource_group_name = azurerm_resource_group.rg.name
  namespace_name = azurerm_servicebus_namespace.sb.name
  topic_name = azurerm_servicebus_topic.sb_topic_msg_processed.name
  max_delivery_count = 10
  default_message_ttl = "P14D"
  auto_delete_on_idle = "P14D"
  dead_lettering_on_filter_evaluation_error = true
  dead_lettering_on_message_expiration = true
}

resource "azurerm_cosmosdb_account" "dba" {
  name = var.cosmosdb_account_name
  location = var.resources_location
  resource_group_name = azurerm_resource_group.rg.name
  offer_type = "Standard"
  kind = "MongoDB"
}

resource "azurerm_cosmosdb_mongo_database" "db" {
  name = var.cosmosdb_database_name
  resource_group_name = azurerm_resource_group.rg.name
  account_name = azurerm_cosmosdb_account.dba.name
}

resource "azurerm_cosmosdb_mongo_collection" "dbc" {
  name = var.cosmosdb_database_collection_name
  resource_group_name = azurerm_resource_group.rg.name
  account_name = azurerm_cosmosdb_account.dba.name
  database_name = azurerm_cosmosdb_mongo_database.db.name
}