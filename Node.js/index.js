const express = require('express')
const bodyParser = require('body-parser')
const app = express();
const { getRenderApps } = require('./RenderApps')

app.use(bodyParser.json())

app.get('/', async (req, res) => {
    try {
        const apps = await getRenderApps(); // קריאה לפונקציה לקבלת רשימת האפליקציות מ-Render
        res.json(apps); // שליחת הרשימה כ-JSON חזרה ללקוח
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Internal Server Error' });
    }
});

app.listen(3000, () => {
    console.log(`Server is running on port http://localhost:3000`);
});

