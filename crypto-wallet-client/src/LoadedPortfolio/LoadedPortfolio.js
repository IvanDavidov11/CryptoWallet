import React from 'react'
import CoinPortfolio from './CoinPortfolio/CoinPortfolio'
import ClearPortfolio from './ClearPortfolio'

const LoadedPortfolio = ({ coins, setFileUploaded}) => {
    return (
        <div className='loadedPortfolio'>
            <ClearPortfolio setFileUploaded={setFileUploaded}/>
            <CoinPortfolio coins={coins} />
        </div>
    )
}

export default LoadedPortfolio
