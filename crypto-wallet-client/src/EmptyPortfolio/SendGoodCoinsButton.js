import React from 'react'

const SendGoodCoinsButton = ({ goodCoins, onFileUpload }) => {
    const uploadSafe_ApiUrl = "https://localhost:7038/api/coins/upload-safe";
    const sendGoodCoins = async () => {
        try {
            const response = await fetch(uploadSafe_ApiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(goodCoins),
            });

            if (response.ok) {
                const result = await response.text();
                console.log(result);
                onFileUpload();
            } else {
                const error = await response.text();
                console.error(error);
            }
        } catch (err) {
            console.error("An error occurred while sending good coins:", err);
        }
    };

    return (
        <button onClick={sendGoodCoins}>Send Good Coins</button>
    )
}

export default SendGoodCoinsButton