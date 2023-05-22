﻿namespace ChatApplication.Infrastructure.Configurations;

using Confluent.Kafka;

public class ConsumerConfiguration : ConsumerConfig
{
    public string MessageTopic { get; set; }
}
