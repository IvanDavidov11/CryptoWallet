import React from 'react'
import CoinTable from './CoinTable'
import PortfolioRefreshSettings from '../PortfolioRefreshSettings'

const CoinPortfolio = ({ coins, fetchCoins }) => {
  const hasApiCalculations = true;

  return (
    <>
      <PortfolioRefreshSettings fetchCoins={fetchCoins} />
      <CoinTable coins={coins} tableCaption={'Your Owned Coins'} hasApiCalculations={hasApiCalculations} />
    </>
  )
}

export default CoinPortfolio