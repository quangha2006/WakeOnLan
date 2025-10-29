# Wake On Lan

A minimal, multi-project .NET 8 solution that lets you wake PCs on your network (Wake-on-LAN) in two ways:

- Console App — a CLI for scripting/automation.
- WPF App — a simple Windows UI to send Magic Packets. (TODO)
- Web App (TODO)

# Usage (Console App)

You can use the **CLI tool** to send Wake-on-LAN packets from a terminal or script.

```bash
WakeOnLan <option>
```
## Options
```
Option	Description
-I      <ip>  	          IP address of the destination computer.
-S      <subnet>  	      Subnet mask of your network.
-IB     <ip broadcast>    Broadcast IP of the destination computer.
-M      <mac address> 	  MAC address of the destination computer.
```
## Examples:
### 1.Send WOL using only MAC (auto-detect network)
```bash
$ WakeOnLan -M 00-1A-2B-3C-4D-5E
```
### 2.Send WOL using IP and subnet mask
```bash
$ WakeOnLan -I 192.168.1.100 -S 255.255.255.0 -M 00-1A-2B-3C-4D-5E
```
### 3.Send WOL using broadcast IP
```bash
$ WakeOnLan -IB 192.168.1.255 -M 00-1A-2B-3C-4D-5E
```
## Expected output:
```bash
The wake-on-lan packet was successfully sent to IP = 192.168.1.255, MAC = 00-1A-2B-3C-4D-5E
```
# Features

- Send Wake-on-LAN (WOL) Magic Packets (UDP, port 9 by default).

- Accepts MAC in common formats: `AA:BB:CC:DD:EE:FF`, `AA-BB-CC-DD-EE-FF`, `AABBCCDDEEFF`.

- Choose broadcast IP (e.g., `192.168.1.255` or `255.255.255.255`) and port.

- Reusable Core library:

  - `WakeOnLanClient.SendMagicPacketAsync(...)`

  - `NetUtils.ParseMac(...), NetUtils.ValidateIP(...)`

  - `NetUtils.GetDefaultInterface(...)` to discover the system’s default NIC & local IP.

# Requirements

- .NET SDK 8.0

- Windows 10/11 for the WPF app (Console/Core can run on Linux/macOS too).

- On the target PC you want to wake:

  - BIOS/UEFI: Enable Wake-on-LAN / “Wake on Magic Packet”.

  - OS/Driver: Enable WOL on the NIC (Power Management / Advanced tab).

  - The machine must have standby power available (S3/S4/S5 support varies by hardware).

# Tips & Notes

- Broadcast IP choice matters. Many routers block global `255.255.255.255`. Prefer your subnet broadcast, e.g., `192.168.0.255 / 10.0.1.255`.

- Firewall: Allow outbound UDP to the chosen port (typically 9 or 7). Some setups also require allowing directed broadcast on the router.

- Multiple sends: Certain devices respond more reliably if you send the packet 2–3 times.

- Pv6: WOL is generally IPv4-centric at the packet level; the library utilities support IPv6 parsing and interface discovery, but WOL delivery typically relies on IPv4 broadcast within the LAN.

# Troubleshooting

### Target doesn’t wake:

- Verify WOL options are enabled in BIOS/NIC driver.

- Use the subnet broadcast IP.

- Ensure the target machine’s NIC still has power in sleep/shutdown.

- Try port 7 if 9 doesn’t work; send the packet multiple times.

### Multiple NICs / VPNs:

- Confirm which NIC is default (NetUtils.GetDefaultInterface() can help).

- If your PC and the target are on different subnets/VLANs, directed broadcast may be blocked.

# Projects

- WakeOnLan.Wpf — Windows WPF app (UI to send Magic Packets).

- WakeOnLan.Cli — Console app for scripting/automation.

- WakeOnLan.Core — Reusable library (Wake On Lan client + network utilities).