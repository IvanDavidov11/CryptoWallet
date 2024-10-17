import React from 'react'
import CoinTable from '../LoadedPortfolio/CoinPortfolio/CoinTable'
import SendGoodCoinsButton from './SendGoodCoinsButton'


const BadUpload = ({ goodCoins, badCoins, onFileUpload, setIsLoading }) => {
  const hasApiCalculations = false;

  return (
    <>
      <CoinTable
        coins={badCoins}
        tableCaption={'Bad Coins (TEMP)'}
        hasApiCalculations={hasApiCalculations}
      />
      <CoinTable
        coins={goodCoins}
        tableCaption={'Good Coins (TEMP)'}
        hasApiCalculations={hasApiCalculations}
      />
      <SendGoodCoinsButton
      goodCoins={goodCoins}
      onFileUpload={onFileUpload}
      setIsLoading={setIsLoading}
      />
    </>
  )
}

export default BadUpload