import React from 'react'
import CoinTable from './CoinTable'
import PortfolioRefresh from '../PortfolioRefresh/PortfolioRefresh'

const CoinPortfolio = ({ coins, fetchCoins }) => {
  const hasApiCalculations = true;

  return (
    <>
      <PortfolioRefresh fetchCoins={fetchCoins} />
      <CoinTable coins={coins} tableCaption={'Your Owned Coins'} hasApiCalculations={hasApiCalculations} />
    </>
  )
}

export default CoinPortfolio