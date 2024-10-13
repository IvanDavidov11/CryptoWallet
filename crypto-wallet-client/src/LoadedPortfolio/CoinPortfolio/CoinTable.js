import React from 'react'
import CoinRow from './CoinRow'

const CoinTable = ({ coins }) => {
    return (
        <div className='table_component'>

            <table>
                <caption>Your owned coins</caption>
                <thead>
                    <tr>
                        <th>Header 1</th>
                        <th>Header 2</th>
                        <th>Header 3</th>
                    </tr>
                </thead>
                <tbody>
                    {coins.map((coin) =>
                        <CoinRow key={coin.id} coin={coin} />
                    )}
                </tbody>
            </table>
        </div>
    )
}

export default CoinTable
