#Net Protocol#

##Handshake Process##

1: The client sends a `HandshakeMessage` to the IPAddress supplied. 

2: The server will reply with a `ConnectionRequestStatusMessage` either confirming or denying the connection.

3: If the connection is accepted, the client will reply with a `ClientInformationMessage` that will contain:
	
	- the username
	- the authentication token of the client
	- client game version
	- skin data

4: The server will check the authentication token against the public server if the setting is set to `true`. It will 
then verify the username against a private database. Client game version will then be checked. The server will respond 
with a `ConnectionResultMessage` containing any errors if there are any. 

5: The client will send a `ModInformationMessage` for information of the modifications running on the server. 

6: The server will reply with a `ModResponseMessage` with information on the URL of the mods installed. If a URL does not 
exist for the modification, the server admin can set `install_mods_to_client` to true. The client will be allowed to either 
confirm or deny the installation of mods that the server attempts to install. Server-side modification installation will be 
explained in another section.

7: Once the client has installed the modifications (if any), it will respond with a `ClientReadyMessage` informing the server
that the handshake process is completed.

##Gameplay Process - Initialization##

1: The server will send entity information of the player using an `PlayerEntityMessage` which the client will match up with it's 
own entity. 

2: The client will send a `PlayerInformationMessage` which only contains the skin data of the client. 

3: The server will send a `EntityUpdateMessage` to all connected clients then a `WorldInformationMessage` to the client. 

4: The server will then begin sending `ChunkDataMessage`s to the client which will be parsed accordingly. 

5: The server will send a `SpawnReadyMessage` to the client which will spawn the client in the requested position. 

6: The server message update loop will begin and add the client to the list of clients to be updated on world events. 

##Gameplay Process - Block Updates##

For any block updates made (ie: breaking, placing, updating), it will all be handled server-side. 
	- BlockAction events will send a `BlockActionMessage`.
	- BlockBreaking events will send a `BlockBreakingMessage`.
	- BlockUpdating will not be sent from the client. They will be sent from the server to inform of block updates. 

##Gameplay Process - Entity Updates## 

For any entity updates made (ie: spawning an entity, striking an entity, using an entity), it will all be handled server-side.
	- EntityStrike events will send an `EntityStrikeMessage`.
	- EntityInteraction events will send an `EntityInteractionMessage`.
	- EntityUpdate events will not be sent from the client. They will be sent from the server to inform of entity updates. 

##Gameplay Process - Animation Messages##

All animation updates will send an `AnimateEntityMessage` to the server to which the server will broadcast the same message to 
connected clients. Valid interactions will be: moving, action, interaction, jumping, death, etc. Any `AnimateEntityMessage` sent 
that does not involve the requested client's entity will be ignored. 

##Gameplay Process - The Game Loop##

The server will be required to send a `KeepAliveMessage` after a connection has not sent a message within the set time. This can
be adjusted with the option `keep_alive_watch`. The default value is `2s` (2 seconds) but can be adjusted for higher player
counts as too many `KeepAliveMessage`s can slow down the message queue.

If the server is running in a dedicated process, it will create it's own game-loop that will have a time delta optimized 
for the system it is running on. If not in a dedicated process, it will use the supplied game-loop that will process the 
message queue optimized for the system. It will attempt to process all queue contents within a single update loop using 
parallel programming but as with all software, it may not work as fast as expected. It is then recommended that use of a
dedicated server is target. Server options can also determine the message cap of the update loop: 

`message_queue_cap`

This is an optional setting with the default being optimized to the system. Valid values are numerical between 10 and 10_000
(ten and ten thousand). This is only the cap, so if a queue does not meet this length, it will still process the messages. 
To use the optimized setting, simply use the value `default`. 