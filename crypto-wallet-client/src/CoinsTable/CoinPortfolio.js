import React from 'react'
import CoinsTable from './CoinsTable';
const CoinPortfolio = ({ coins, setFileUploaded, checkIfHasCoins }) => {

  return (
    <>
      <CoinsTable
      coins={coins}
      tableCaption={'Your Positions'}
      setFileUploaded={setFileUploaded}
      checkIfHasCoins={checkIfHasCoins}
      />
    </>
  )
}

export default CoinPortfolio