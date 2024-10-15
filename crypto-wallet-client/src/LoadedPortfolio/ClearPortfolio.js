import React from 'react'

const ClearPortfolio = ({ setFileUploaded }) => {
    const clearCoins_ApiUrl = "https://localhost:7038/api/coins/clear";

    const clearPortfolio = async () => {
        try {
            const response = await fetch(clearCoins_ApiUrl, { method: 'DELETE' });

            if (response.ok) {
                setFileUploaded(false);
                console.log('Portfolio cleared');
            }
        } catch (error) {
            console.error('Error clearing portfolio:', error);
        }
    };

    return (
        <div>
            <button onClick={clearPortfolio}>Clear Portfolio</button>
        </div>
    )
}

export default ClearPortfolio