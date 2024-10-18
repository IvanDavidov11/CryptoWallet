import React from 'react'

const DeletePortfolio = ({ setFileUploaded, checkIfHasCoins }) => {
    const clearCoins_ApiUrl = "https://localhost:7038/api/coins/clear";

    const deletePortfolio = async () => {
        try {
            const response = await fetch(clearCoins_ApiUrl, { method: 'DELETE' });

            if (response.ok) {
                setFileUploaded(false);
                checkIfHasCoins();
                console.log('Portfolio cleared');
            }
        } catch (error) {
            console.error('Error clearing portfolio:', error);
        }
    };

    return (
        <>
            <button className='deletePortfolioButton' onClick={deletePortfolio}>Delete Portfolio</button>
        </>
    )
}

export default DeletePortfolio