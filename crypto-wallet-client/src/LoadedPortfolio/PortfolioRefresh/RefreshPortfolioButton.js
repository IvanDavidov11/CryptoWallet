import React from 'react'

const RefreshPortfolioButton = ({ fetchCoins, fetchCurrentValue }) => {

    const handlePortfolioReload = () => {
        fetchCoins();
        fetchCurrentValue();
    };

    return (
        <>
            <button className='refreshPortfolioButton' onClick={handlePortfolioReload}>Update Now</button>
        </>
    )
}

export default RefreshPortfolioButton