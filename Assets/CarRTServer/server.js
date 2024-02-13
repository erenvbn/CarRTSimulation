const WebSocket = require("ws");

// Create a WebSocket server
const wss = new WebSocket.Server({ port: 5005 });

// Function to generate a random number within a range
function getRandomNumber(min, max) {
    return Math.random() * (max - min) + min;
}

// Function to send serialized fields data as JSON
function sendSerializedFieldsData(ws) {
    const speed = getRandomNumber(2, 6); // Replace 100 with your maximum speed value
    const rotation = getRandomNumber(45, 45); // Replace 360 with your maximum rotation value
    const data = JSON.stringify({ speed, rotation });
    ws.send(data);
}

// Send serialized fields data every second
setInterval(() => {
    wss.clients.forEach((client) => {
        if (client.readyState === WebSocket.OPEN) {
            sendSerializedFieldsData(client);
        }
    });
}, 1000);

// WebSocket server listening
wss.on("listening", () => {
    console.log("WebSocket server is running on port 5005");
});

// WebSocket server message handling
wss.on("connection", (ws) => {
    console.log("Client connected");

    // Message event handler
    ws.on("message", (message) => {
        try {
            const data = JSON.parse(message);
            console.log("Parsed message:", data);
        } catch (error) {
            console.error("Error parsing incoming message:", error);
        }
    });

    // Close event handler
    ws.on("close", () => {
        console.log("Client disconnected");
    });
});
