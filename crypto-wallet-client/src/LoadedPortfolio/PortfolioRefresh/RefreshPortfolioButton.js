import React from 'react'

const RefreshPortfolioButton = ({ fetchCoins }) => {

    const handleCoinsReload = () => {
        fetchCoins();
    };

    return (
        <div>
            <button onClick={handleCoinsReload}>Update Portfolio</button>
        </div>
    )
}

export default RefreshPortfolioButton
