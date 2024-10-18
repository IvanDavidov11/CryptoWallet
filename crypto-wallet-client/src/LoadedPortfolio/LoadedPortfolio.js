import React, { useState } from 'react'
import LoadedPortfolioHeader from './LoadedPortfolioHeader'
import CoinPortfolio from '../CoinsTable/CoinPortfolio'
import InitialPortfolioValue from './InitialPortfolioValue'
import CurrentPortfolioValue from './CurrentPortfolioValue'
import PortfolioRefresh from './PortfolioRefresh/PortfolioRefresh';

const LoadedPortfolio = ({ coins, setFileUploaded, fetchCoins, checkIfHasCoins }) => {
    const calculateCurrent_ApiUrl = "https://localhost:7038/api/calc/current";
    const [currentValue, setCurrentValue] = useState(0);

    const fetchCurrentValue = async () => {
        try {
            const response = await fetch(calculateCurrent_ApiUrl);

            if (!response.ok) throw Error('Did not receive expected data');

            const currentValue = await response.text();
            setCurrentValue(currentValue);
        }
        catch (err) {
            console.log(err);
        }
    }

    return (
        <div className='loadedPortfolio'>
            <LoadedPortfolioHeader fetchCoins={fetchCoins} />
            <PortfolioRefresh fetchCoins={fetchCoins} fetchCurrentValue={fetchCurrentValue} />
            <div className='portfolio-infoboxes'>
                <InitialPortfolioValue />
                <CurrentPortfolioValue currentValue={currentValue} fetchCurrentValue={fetchCurrentValue} />
            </div>
            <CoinPortfolio coins={coins} setFileUploaded={setFileUploaded} checkIfHasCoins={checkIfHasCoins} />
        </div>
    );
}

export default LoadedPortfolio