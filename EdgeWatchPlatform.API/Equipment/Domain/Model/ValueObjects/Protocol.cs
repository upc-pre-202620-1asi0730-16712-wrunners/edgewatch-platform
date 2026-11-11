namespace EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

/// <summary>Industrial protocol supported by a controller (ETHERNET_IP, OPC_UA, MODBUS_TCP, S7).</summary>
public enum Protocol { EthernetIp, OpcUa, ModbusTcp, S7 }
