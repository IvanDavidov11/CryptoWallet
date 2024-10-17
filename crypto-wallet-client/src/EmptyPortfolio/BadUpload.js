import React from 'react'
import CoinsTable from '../CoinsTable/CoinsTable';
import SendGoodCoinsButton from './SendGoodCoinsButton'


const BadUpload = ({ goodCoins, badCoins, onFileUpload, setIsLoading }) => {
  const hasApiCalculations = false;

  return (
    <>
      <CoinsTable
        coins={badCoins}
        tableCaption={'Bad Coins (TEMP)'}
        hasApiCalculations={hasApiCalculations}
      />
      <CoinsTable
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