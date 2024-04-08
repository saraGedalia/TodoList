const axios = require('axios');

async function getRenderApps() {
    const apiKey = 'rnd_xlOmygyKuWU3HLUcyEHUeOxZNK8O'; // עליך להחליף את זה במפתח ה-API שלך

    try {
        const response = await axios.get('https://api.render.com/v1/services?limit=20', {
            headers: {
                'accept': 'application/json',
                'authorization': `Bearer ${apiKey}`
            }
        });
        return response.data;
    } catch (error) {
        console.error(error);
        return null;
    }
}

module.exports = { getRenderApps };
