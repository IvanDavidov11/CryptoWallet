import React from 'react'

const SendAllCoinsForDeeperSearchButton = ({ goodCoins, setGoodCoins, badCoins, setBadCoins, setUploadFormatFailed, onFileUpload, }) => {
    const uploadDeep_ApiUrl = "https://localhost:7038/api/coins/upload-deep";

    const sendAllCoins = async () => {
        if (!goodCoins || !badCoins || (goodCoins.length === 0 && badCoins.length === 0)) {
            console.error('No coins to send.');
        }

        try {
            const payload = {
                goodCoins,
                badCoins
            };

            const response = await fetch(uploadDeep_ApiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(payload),
            });

            if (response.status === 206) {
                const result = await response.json();
                setBadCoins(result.badCoins);
                setGoodCoins(result.goodCoins);
                setUploadFormatFailed(true);
            }
            else if (response.ok) {
                const data = await response.text();
                console.log('Success:', data);
                onFileUpload()
            }
        } catch (error) {
            console.error('Failed to send coins:', error);
        }
        finally{
            //add logic for is loading
        }
        
    };

    return (
        <button onClick={sendAllCoins}>Dig Deeper</button>
    )
}

export default SendAllCoinsForDeeperSearchButton
